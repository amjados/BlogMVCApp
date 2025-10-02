using Xunit;
using FluentAssertions;
using BlogMVCApp.Models;

namespace BlogMVCApp.Tests.Models
{
    public class BlogModelsTests
    {
        public class CategoryTests
        {
            [Fact]
            public void Category_DefaultValues_ShouldBeSetCorrectly()
            {
                // Act
                var category = new Category();

                // Assert
                category.Name.Should().Be(string.Empty);
                category.Slug.Should().Be(string.Empty);
                category.CreatedBy.Should().Be(string.Empty);
                category.IsActive.Should().BeTrue();
                category.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
                category.Posts.Should().NotBeNull();
                category.Posts.Should().BeEmpty();
            }

            [Fact]
            public void Category_WithValidData_ShouldSetPropertiesCorrectly()
            {
                // Arrange
                var testDate = DateTime.UtcNow;

                // Act
                var category = new Category
                {
                    Id = 1,
                    Name = "Technology",
                    Description = "Tech related posts",
                    Slug = "technology",
                    CreatedBy = "admin",
                    IsActive = true,
                    CreatedAt = testDate
                };

                // Assert
                category.Id.Should().Be(1);
                category.Name.Should().Be("Technology");
                category.Description.Should().Be("Tech related posts");
                category.Slug.Should().Be("technology");
                category.CreatedBy.Should().Be("admin");
                category.IsActive.Should().BeTrue();
                category.CreatedAt.Should().Be(testDate);
            }
        }

        public class PostTests
        {
            [Fact]
            public void Post_DefaultValues_ShouldBeSetCorrectly()
            {
                // Act
                var post = new Post();

                // Assert
                post.Title.Should().Be(string.Empty);
                post.Content.Should().Be(string.Empty);
                post.Slug.Should().Be(string.Empty);
                post.AuthorId.Should().Be(string.Empty);
                post.Status.Should().Be("Draft");
                post.ViewCount.Should().Be(0);
                post.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
                post.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
                post.Comments.Should().NotBeNull();
                post.Comments.Should().BeEmpty();
                post.PostTags.Should().NotBeNull();
                post.PostTags.Should().BeEmpty();
            }

            [Fact]
            public void Post_WithValidData_ShouldSetPropertiesCorrectly()
            {
                // Arrange
                var testDate = DateTime.UtcNow;
                var publishDate = DateTime.UtcNow.AddDays(1);

                // Act
                var post = new Post
                {
                    Id = 1,
                    Title = "Test Post",
                    Content = "This is test content",
                    Excerpt = "Test excerpt",
                    Slug = "test-post",
                    FeaturedImageUrl = "https://example.com/image.jpg",
                    CategoryId = 1,
                    AuthorId = "user123",
                    Status = "Published",
                    PublishedAt = publishDate,
                    CreatedAt = testDate,
                    UpdatedAt = testDate,
                    ViewCount = 100,
                    MetaTitle = "Test Meta Title",
                    MetaDescription = "Test meta description"
                };

                // Assert
                post.Id.Should().Be(1);
                post.Title.Should().Be("Test Post");
                post.Content.Should().Be("This is test content");
                post.Excerpt.Should().Be("Test excerpt");
                post.Slug.Should().Be("test-post");
                post.FeaturedImageUrl.Should().Be("https://example.com/image.jpg");
                post.CategoryId.Should().Be(1);
                post.AuthorId.Should().Be("user123");
                post.Status.Should().Be("Published");
                post.PublishedAt.Should().Be(publishDate);
                post.ViewCount.Should().Be(100);
                post.MetaTitle.Should().Be("Test Meta Title");
                post.MetaDescription.Should().Be("Test meta description");
            }
        }

        public class CommentTests
        {
            [Fact]
            public void Comment_DefaultValues_ShouldBeSetCorrectly()
            {
                // Act
                var comment = new Comment();

                // Assert
                comment.AuthorName.Should().Be(string.Empty);
                comment.Content.Should().Be(string.Empty);
                comment.IsApproved.Should().BeFalse();
                comment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
                comment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
                comment.Replies.Should().NotBeNull();
                comment.Replies.Should().BeEmpty();
            }

            [Fact]
            public void Comment_WithValidData_ShouldSetPropertiesCorrectly()
            {
                // Arrange
                var testDate = DateTime.UtcNow;

                // Act
                var comment = new Comment
                {
                    Id = 1,
                    PostId = 1,
                    AuthorId = "user123",
                    AuthorName = "John Doe",
                    AuthorEmail = "john@example.com",
                    Content = "Great post!",
                    ParentCommentId = null,
                    IsApproved = true,
                    CreatedAt = testDate,
                    UpdatedAt = testDate,
                    IpAddress = "192.168.1.1"
                };

                // Assert
                comment.Id.Should().Be(1);
                comment.PostId.Should().Be(1);
                comment.AuthorId.Should().Be("user123");
                comment.AuthorName.Should().Be("John Doe");
                comment.AuthorEmail.Should().Be("john@example.com");
                comment.Content.Should().Be("Great post!");
                comment.ParentCommentId.Should().BeNull();
                comment.IsApproved.Should().BeTrue();
                comment.IpAddress.Should().Be("192.168.1.1");
            }
        }

        public class TagTests
        {
            [Fact]
            public void Tag_DefaultValues_ShouldBeSetCorrectly()
            {
                // Act
                var tag = new Tag();

                // Assert
                tag.Name.Should().Be(string.Empty);
                tag.Slug.Should().Be(string.Empty);
                tag.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
                tag.PostTags.Should().NotBeNull();
                tag.PostTags.Should().BeEmpty();
            }

            [Fact]
            public void Tag_WithValidData_ShouldSetPropertiesCorrectly()
            {
                // Arrange
                var testDate = DateTime.UtcNow;

                // Act
                var tag = new Tag
                {
                    Id = 1,
                    Name = "ASP.NET",
                    Slug = "aspnet",
                    CreatedAt = testDate
                };

                // Assert
                tag.Id.Should().Be(1);
                tag.Name.Should().Be("ASP.NET");
                tag.Slug.Should().Be("aspnet");
                tag.CreatedAt.Should().Be(testDate);
            }
        }

        public class PostTagTests
        {
            [Fact]
            public void PostTag_ShouldSetPropertiesCorrectly()
            {
                // Act
                var postTag = new PostTag
                {
                    PostId = 1,
                    TagId = 2
                };

                // Assert
                postTag.PostId.Should().Be(1);
                postTag.TagId.Should().Be(2);
            }
        }

        public class UserSessionTests
        {
            [Fact]
            public void UserSession_DefaultValues_ShouldBeSetCorrectly()
            {
                // Act
                var session = new UserSession();

                // Assert
                session.Id.Should().NotBe(Guid.Empty);
                session.UserId.Should().Be(string.Empty);
                session.SessionToken.Should().Be(string.Empty);
                session.IsActive.Should().BeTrue();
                session.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
                session.LastAccessedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            }

            [Fact]
            public void UserSession_WithValidData_ShouldSetPropertiesCorrectly()
            {
                // Arrange
                var testDate = DateTime.UtcNow;
                var expiryDate = DateTime.UtcNow.AddHours(24);

                // Act
                var session = new UserSession
                {
                    UserId = "user123",
                    SessionToken = "token123",
                    DeviceInfo = "Windows PC",
                    IpAddress = "192.168.1.1",
                    UserAgent = "Mozilla/5.0",
                    IsActive = true,
                    CreatedAt = testDate,
                    LastAccessedAt = testDate,
                    ExpiresAt = expiryDate
                };

                // Assert
                session.UserId.Should().Be("user123");
                session.SessionToken.Should().Be("token123");
                session.DeviceInfo.Should().Be("Windows PC");
                session.IpAddress.Should().Be("192.168.1.1");
                session.UserAgent.Should().Be("Mozilla/5.0");
                session.IsActive.Should().BeTrue();
                session.ExpiresAt.Should().Be(expiryDate);
            }
        }

        public class AuditLogTests
        {
            [Fact]
            public void AuditLog_DefaultValues_ShouldBeSetCorrectly()
            {
                // Act
                var auditLog = new AuditLog();

                // Assert
                auditLog.Action.Should().Be(string.Empty);
                auditLog.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            }

            [Fact]
            public void AuditLog_WithValidData_ShouldSetPropertiesCorrectly()
            {
                // Arrange
                var testDate = DateTime.UtcNow;

                // Act
                var auditLog = new AuditLog
                {
                    Id = 1,
                    UserId = "user123",
                    Action = "CREATE",
                    EntityType = "Post",
                    EntityId = "1",
                    OldValues = null,
                    NewValues = "{\"Title\":\"New Post\"}",
                    IpAddress = "192.168.1.1",
                    UserAgent = "Mozilla/5.0",
                    Timestamp = testDate
                };

                // Assert
                auditLog.Id.Should().Be(1);
                auditLog.UserId.Should().Be("user123");
                auditLog.Action.Should().Be("CREATE");
                auditLog.EntityType.Should().Be("Post");
                auditLog.EntityId.Should().Be("1");
                auditLog.OldValues.Should().BeNull();
                auditLog.NewValues.Should().Be("{\"Title\":\"New Post\"}");
                auditLog.IpAddress.Should().Be("192.168.1.1");
                auditLog.UserAgent.Should().Be("Mozilla/5.0");
                auditLog.Timestamp.Should().Be(testDate);
            }
        }
    }
}