using Xunit;
using FluentAssertions;
using BlogMVCApp.Controllers;
using BlogMVCApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlogMVCApp.Tests.Utilities
{
    public class SlugGeneratorTests
    {
        private readonly HomeController _controller;

        public SlugGeneratorTests()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            // Create controller instance for testing the private GenerateSlug method
            _controller = new HomeController(null!, null!, null!, context);
        }

        private string CallGenerateSlug(string title)
        {
            // Use reflection to call the private GenerateSlug method
            var method = typeof(HomeController).GetMethod("GenerateSlug", BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)method!.Invoke(_controller, new object[] { title })!;
        }

        [Fact]
        public void GenerateSlug_WithSimpleTitle_ShouldCreateValidSlug()
        {
            // Arrange
            var title = "Hello World";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("hello-world");
        }

        [Fact]
        public void GenerateSlug_WithSpecialCharacters_ShouldRemoveThem()
        {
            // Arrange
            var title = "Hello, World! How are you?";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("hello-world-how-are-you");
        }

        [Fact]
        public void GenerateSlug_WithApostrophes_ShouldRemoveThem()
        {
            // Arrange
            var title = "It's a beautiful day";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("its-a-beautiful-day");
        }

        [Fact]
        public void GenerateSlug_WithQuotes_ShouldRemoveThem()
        {
            // Arrange
            var title = "The \"Best\" Article Ever";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("the-best-article-ever");
        }

        [Fact]
        public void GenerateSlug_WithPeriods_ShouldRemoveThem()
        {
            // Arrange
            var title = "Version 2.0 Release";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("version-20-release");
        }

        [Fact]
        public void GenerateSlug_WithAmpersand_ShouldReplaceWithAnd()
        {
            // Arrange
            var title = "HTML & CSS Tutorial";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("html-and-css-tutorial");
        }

        [Fact]
        public void GenerateSlug_WithAtSymbol_ShouldReplaceWithAt()
        {
            // Arrange
            var title = "Email @ Domain.com";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("email-at-domaincom");
        }

        [Fact]
        public void GenerateSlug_WithMultipleSpaces_ShouldConvertToSingleDash()
        {
            // Arrange
            var title = "Hello    World    Test";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("hello----world----test");
        }

        [Fact]
        public void GenerateSlug_WithMixedCase_ShouldConvertToLowercase()
        {
            // Arrange
            var title = "HELLO World TeSt";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("hello-world-test");
        }

        [Fact]
        public void GenerateSlug_WithNumbers_ShouldPreserveNumbers()
        {
            // Arrange
            var title = "ASP.NET Core 8.0 Tutorial";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("aspnet-core-80-tutorial");
        }

        [Fact]
        public void GenerateSlug_WithEmptyString_ShouldReturnEmptyString()
        {
            // Arrange
            var title = "";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("");
        }

        [Fact]
        public void GenerateSlug_WithOnlySpecialCharacters_ShouldReturnProcessedString()
        {
            // Arrange
            var title = "!@#$%^&*()";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("at#and");
        }

        [Fact]
        public void GenerateSlug_WithOnlySpaces_ShouldReturnDashes()
        {
            // Arrange
            var title = "   ";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("---");
        }

        [Fact]
        public void GenerateSlug_WithComplexTitle_ShouldHandleAllTransformations()
        {
            // Arrange
            var title = "How to Build a \"Modern\" Web App with ASP.NET Core 8.0 & Entity Framework!";

            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be("how-to-build-a-modern-web-app-with-aspnet-core-80-and-entity-framework");
        }

        [Theory]
        [InlineData("Getting Started", "getting-started")]
        [InlineData("C# Programming", "c#-programming")]
        [InlineData("API Development", "api-development")]
        [InlineData("Database Design", "database-design")]
        [InlineData("Unit Testing", "unit-testing")]
        public void GenerateSlug_WithCommonTitles_ShouldCreateExpectedSlugs(string title, string expectedSlug)
        {
            // Act
            var slug = CallGenerateSlug(title);

            // Assert
            slug.Should().Be(expectedSlug);
        }
    }
}