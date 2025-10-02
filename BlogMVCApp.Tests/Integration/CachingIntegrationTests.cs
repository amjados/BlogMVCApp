using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using BlogMVCApp.Data;
using BlogMVCApp.Services;
using BlogMVCApp.Models;

namespace BlogMVCApp.Tests.Integration
{
    public class CachingIntegrationTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        public CachingIntegrationTests()
        {
            // Setup test services
            var services = new ServiceCollection();

            // Add logging
            services.AddLogging(builder => builder.AddConsole());

            // Add memory cache
            services.AddMemoryCache();

            // Add in-memory database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

            // Add our services
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<BlogService>();
            services.AddScoped<IBlogService>(provider =>
            {
                var concreteService = provider.GetRequiredService<BlogService>();
                var cacheService = provider.GetRequiredService<ICacheService>();
                var logger = provider.GetRequiredService<ILogger<CachedBlogService>>();
                return new CachedBlogService(concreteService, cacheService, logger);
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task CachedBlogService_ShouldCacheResults()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var blogService = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

            // Create a test category
            var category = new Category
            {
                Name = "Test Category",
                Slug = "test-category",
                CreatedBy = "test-user",
                IsActive = true
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act - First call should hit the database
            var categories1 = await blogService.GetActiveCategoriesAsync();

            // Act - Second call should hit the cache
            var categories2 = await blogService.GetActiveCategoriesAsync();

            // Assert
            categories1.Should().NotBeNull();
            categories2.Should().NotBeNull();
            categories1.Should().HaveCount(1);
            categories2.Should().HaveCount(1);

            // Verify cache contains the data
            var cachedData = await cacheService.GetAsync<IEnumerable<Category>>("categories:active");
            cachedData.Should().NotBeNull();
            cachedData!.Should().HaveCount(1);
        }

        [Fact]
        public async Task CacheService_ShouldExpireItems()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

            var testData = "test value";
            var expiry = TimeSpan.FromMilliseconds(100);

            // Act
            await cacheService.SetAsync("test_key", testData, expiry);

            // Assert - Should be available immediately
            var cachedValue1 = await cacheService.GetAsync<string>("test_key");
            cachedValue1.Should().Be(testData);

            // Wait for expiry
            await Task.Delay(150);

            // Assert - Should be expired
            var cachedValue2 = await cacheService.GetAsync<string>("test_key");
            cachedValue2.Should().BeNull();
        }

        [Fact]
        public async Task CacheService_ShouldHandleInvalidation()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

            var testData = "test value";

            // Act
            await cacheService.SetAsync("test_key", testData, TimeSpan.FromMinutes(10));

            // Verify it's cached
            var cachedValue1 = await cacheService.GetAsync<string>("test_key");
            cachedValue1.Should().Be(testData);

            // Remove from cache
            await cacheService.RemoveAsync("test_key");

            // Assert - Should be removed
            var cachedValue2 = await cacheService.GetAsync<string>("test_key");
            cachedValue2.Should().BeNull();
        }

        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }
    }
}