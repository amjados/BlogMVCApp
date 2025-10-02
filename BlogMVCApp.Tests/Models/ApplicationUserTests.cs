using Xunit;
using FluentAssertions;
using BlogMVCApp.Models;

namespace BlogMVCApp.Tests.Models
{
    public class ApplicationUserTests
    {
        [Fact]
        public void ApplicationUser_DefaultValues_ShouldBeSetCorrectly()
        {
            // Act
            var user = new ApplicationUser();

            // Assert
            user.IsActive.Should().BeTrue();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            user.Posts.Should().NotBeNull();
            user.Posts.Should().BeEmpty();
            user.Comments.Should().NotBeNull();
            user.Comments.Should().BeEmpty();
            user.Categories.Should().NotBeNull();
            user.Categories.Should().BeEmpty();
            user.UserSessions.Should().NotBeNull();
            user.UserSessions.Should().BeEmpty();
            user.AuditLogs.Should().NotBeNull();
            user.AuditLogs.Should().BeEmpty();
        }

        [Fact]
        public void ApplicationUser_WithValidData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var testDate = DateTime.UtcNow;

            // Act
            var user = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Biography = "Software Developer",
                ProfileImageUrl = "https://example.com/profile.jpg",
                CreatedAt = testDate,
                UpdatedAt = testDate,
                IsActive = true
            };

            // Assert
            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Doe");
            user.Email.Should().Be("john.doe@example.com");
            user.UserName.Should().Be("johndoe");
            user.Biography.Should().Be("Software Developer");
            user.ProfileImageUrl.Should().Be("https://example.com/profile.jpg");
            user.CreatedAt.Should().Be(testDate);
            user.UpdatedAt.Should().Be(testDate);
            user.IsActive.Should().BeTrue();
        }

        [Fact]
        public void FullName_WithBothNames_ShouldCombineThem()
        {
            // Arrange
            var user = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("John Doe");
        }

        [Fact]
        public void FullName_WithFirstNameOnly_ShouldReturnFirstName()
        {
            // Arrange
            var user = new ApplicationUser
            {
                FirstName = "John",
                LastName = null
            };

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("John");
        }

        [Fact]
        public void FullName_WithLastNameOnly_ShouldReturnLastName()
        {
            // Arrange
            var user = new ApplicationUser
            {
                FirstName = null,
                LastName = "Doe"
            };

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("Doe");
        }

        [Fact]
        public void FullName_WithEmptyNames_ShouldReturnEmptyString()
        {
            // Arrange
            var user = new ApplicationUser
            {
                FirstName = "",
                LastName = ""
            };

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("");
        }

        [Fact]
        public void FullName_WithNullNames_ShouldReturnEmptyString()
        {
            // Arrange
            var user = new ApplicationUser
            {
                FirstName = null,
                LastName = null
            };

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("");
        }

        [Fact]
        public void FullName_WithWhitespaceNames_ShouldTrimCorrectly()
        {
            // Arrange
            var user = new ApplicationUser
            {
                FirstName = "  John  ",
                LastName = "  Doe  "
            };

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("John     Doe");
        }

        [Fact]
        public void ApplicationUser_InheritsFromIdentityUser_ShouldHaveIdentityProperties()
        {
            // Arrange & Act
            var user = new ApplicationUser();

            // Assert
            user.Should().BeAssignableTo<Microsoft.AspNetCore.Identity.IdentityUser>();
            // Check that inherited properties are available
            user.Id.Should().NotBeNull();
            user.Email.Should().BeNull();
            user.UserName.Should().BeNull();
            user.EmailConfirmed.Should().BeFalse();
            user.PhoneNumberConfirmed.Should().BeFalse();
            user.TwoFactorEnabled.Should().BeFalse();
            user.LockoutEnabled.Should().BeTrue();
            user.AccessFailedCount.Should().Be(0);
        }
    }
}