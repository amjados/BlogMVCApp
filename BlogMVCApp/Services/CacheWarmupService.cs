using BlogMVCApp.Services;

namespace BlogMVCApp.Services
{
    public interface ICacheWarmupService
    {
        Task WarmupCacheAsync();
    }

    public class CacheWarmupService : ICacheWarmupService
    {
        private readonly IBlogService _blogService;
        private readonly ILogger<CacheWarmupService> _logger;

        public CacheWarmupService(IBlogService blogService, ILogger<CacheWarmupService> logger)
        {
            _blogService = blogService;
            _logger = logger;
        }

        public async Task WarmupCacheAsync()
        {
            try
            {
                _logger.LogInformation("Starting cache warmup...");

                // Warm up categories
                await WarmupCategories();

                // Warm up published posts (first page)
                await WarmupPublishedPosts();

                // Warm up tags
                await WarmupTags();

                _logger.LogInformation("Cache warmup completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cache warmup");
            }
        }

        private async Task WarmupCategories()
        {
            try
            {
                _logger.LogDebug("Warming up categories cache...");
                await _blogService.GetActiveCategoriesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error warming up categories cache");
            }
        }

        private async Task WarmupPublishedPosts()
        {
            try
            {
                _logger.LogDebug("Warming up published posts cache...");
                // Warm up first few pages
                await _blogService.GetPublishedPostsAsync(1, 10);
                await _blogService.GetPublishedPostsAsync(2, 10);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error warming up published posts cache");
            }
        }

        private async Task WarmupTags()
        {
            try
            {
                _logger.LogDebug("Warming up tags cache...");
                await _blogService.GetTagsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error warming up tags cache");
            }
        }
    }
}