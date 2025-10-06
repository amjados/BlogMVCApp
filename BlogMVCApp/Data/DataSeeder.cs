using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogMVCApp.Models;

namespace BlogMVCApp.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger logger)
        {
            try
            {
                await SeedRolesAsync(roleManager, logger);
                await SeedUsersAsync(userManager, logger);
                await SeedCategoriesAsync(context, logger);
                await SeedTagsAsync(context, logger);
                await SeedPostsAsync(context, logger);
                await SeedCommentsAsync(context, logger);

                await context.SaveChangesAsync();
                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager, ILogger logger)
        {
            try
            {
                var roles = new[]
                {
                    new { Name = "Admin", Description = "Full system administration access" },
                    new { Name = "Editor", Description = "Can create and edit posts" },
                    new { Name = "Author", Description = "Can create own posts" },
                    new { Name = "Subscriber", Description = "Can comment and subscribe" }
                };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        var applicationRole = new ApplicationRole
                        {
                            Name = role.Name,
                            Description = role.Description,
                            CreatedAt = DateTime.UtcNow
                        };

                        await roleManager.CreateAsync(applicationRole);
                        logger.LogInformation($"Created role: {role.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding roles");
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            try
            {
                var users = new[]
                {
                    new { Email = "admin@blogmvc.com", Password = "Admin123!", FirstName = "Admin", LastName = "User", Biography = "System administrator with full access to manage the blog platform.", Role = "Admin" },
                    new { Email = "john.doe@blogmvc.com", Password = "Author123!", FirstName = "John", LastName = "Doe", Biography = "Passionate software developer and tech blogger with over 5 years of experience in .NET development.", Role = "Author" },
                    new { Email = "jane.smith@blogmvc.com", Password = "Editor123!", FirstName = "Jane", LastName = "Smith", Biography = "Content editor specializing in technical writing and developer tutorials.", Role = "Editor" }
                };

                foreach (var userData in users)
                {
                    var existingUser = await userManager.FindByEmailAsync(userData.Email);
                    if (existingUser == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = userData.Email,
                            Email = userData.Email,
                            EmailConfirmed = true,
                            FirstName = userData.FirstName,
                            LastName = userData.LastName,
                            Biography = userData.Biography,
                            CreatedAt = DateTime.UtcNow.AddDays(-30),
                            IsActive = true
                        };

                        var result = await userManager.CreateAsync(user, userData.Password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, userData.Role);
                            logger.LogInformation($"Created user: {userData.Email}");
                        }
                        else
                        {
                            logger.LogWarning($"Failed to create user {userData.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        // For admin user, always reset password to ensure it's correct
                        if (userData.Email == "admin@blogmvc.com")
                        {
                            var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);
                            var result = await userManager.ResetPasswordAsync(existingUser, token, userData.Password);
                            if (result.Succeeded)
                            {
                                logger.LogInformation($"Reset password for admin user: {userData.Email}");
                            }
                            else
                            {
                                logger.LogWarning($"Failed to reset password for {userData.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding users");
            }
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.Categories.AnyAsync())
                {
                    var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@blogmvc.com");
                    if (adminUser != null)
                    {
                        var categories = new[]
                        {
                            new Category { Name = "Programming", Description = "Articles about programming languages, frameworks, and development practices.", Slug = "programming", CreatedBy = adminUser.Id, CreatedAt = DateTime.UtcNow.AddDays(-30), IsActive = true },
                            new Category { Name = "Web Development", Description = "Frontend and backend web development tutorials and guides.", Slug = "web-development", CreatedBy = adminUser.Id, CreatedAt = DateTime.UtcNow.AddDays(-29), IsActive = true },
                            new Category { Name = "Database", Description = "Database design, optimization, and management techniques.", Slug = "database", CreatedBy = adminUser.Id, CreatedAt = DateTime.UtcNow.AddDays(-28), IsActive = true },
                            new Category { Name = "DevOps", Description = "Deployment, CI/CD, containerization, and infrastructure topics.", Slug = "devops", CreatedBy = adminUser.Id, CreatedAt = DateTime.UtcNow.AddDays(-27), IsActive = true }
                        };

                        await context.Categories.AddRangeAsync(categories);
                        await context.SaveChangesAsync();
                        logger.LogInformation("Categories seeded successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding categories");
            }
        }

        private static async Task SeedTagsAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.Tags.AnyAsync())
                {
                    var tags = new[]
                    {
                        new Tag { Name = "ASP.NET Core", Slug = "aspnet-core" },
                        new Tag { Name = "C#", Slug = "csharp" },
                        new Tag { Name = "Entity Framework", Slug = "entity-framework" },
                        new Tag { Name = "Web Development", Slug = "web-development" },
                        new Tag { Name = "Tutorial", Slug = "tutorial" },
                        new Tag { Name = "JavaScript", Slug = "javascript" },
                        new Tag { Name = "SQL Server", Slug = "sql-server" },
                        new Tag { Name = "Docker", Slug = "docker" },
                        new Tag { Name = "Azure", Slug = "azure" },
                        new Tag { Name = "Performance", Slug = "performance" }
                    };

                    await context.Tags.AddRangeAsync(tags);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Tags seeded successfully");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding tags");
            }
        }

        private static async Task SeedPostsAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.Posts.AnyAsync())
                {
                    var authorUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "john.doe@blogmvc.com");
                    var editorUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "jane.smith@blogmvc.com");

                    if (authorUser != null && editorUser != null)
                    {
                        var webDevCategory = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "web-development");
                        var databaseCategory = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "database");
                        var devopsCategory = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "devops");

                        var posts = new[]
                        {
                            new Post
                            {
                                Title = "Getting Started with ASP.NET Core MVC",
                                Content = GetMvcArticleContent(),
                                Excerpt = "Learn the fundamentals of ASP.NET Core MVC and create your first web application with this comprehensive guide.",
                                Slug = "getting-started-aspnet-core-mvc",
                                CategoryId = webDevCategory?.Id,
                                AuthorId = authorUser.Id,
                                Status = "Published",
                                PublishedAt = DateTime.UtcNow.AddDays(-15),
                                CreatedAt = DateTime.UtcNow.AddDays(-16),
                                UpdatedAt = DateTime.UtcNow.AddDays(-15),
                                ViewCount = 1247,
                                MetaTitle = "ASP.NET Core MVC Tutorial - Getting Started Guide",
                                MetaDescription = "Complete beginner's guide to ASP.NET Core MVC with step-by-step instructions and code examples."
                            },
                            new Post
                            {
                                Title = "Entity Framework Core Best Practices",
                                Content = GetEfCoreArticleContent(),
                                Excerpt = "Essential best practices for Entity Framework Core to build performant and maintainable .NET applications.",
                                Slug = "entity-framework-core-best-practices",
                                CategoryId = databaseCategory?.Id,
                                AuthorId = editorUser.Id,
                                Status = "Published",
                                PublishedAt = DateTime.UtcNow.AddDays(-10),
                                CreatedAt = DateTime.UtcNow.AddDays(-12),
                                UpdatedAt = DateTime.UtcNow.AddDays(-10),
                                ViewCount = 892,
                                MetaTitle = "Entity Framework Core Best Practices - Performance & Maintainability",
                                MetaDescription = "Learn essential EF Core best practices for building high-performance .NET applications with proper data access patterns."
                            },
                            new Post
                            {
                                Title = "Modern JavaScript ES2023 Features",
                                Content = GetJavaScriptArticleContent(),
                                Excerpt = "Discover the latest JavaScript ES2023 features including new array methods, hashbang support, and improved WeakMap functionality.",
                                Slug = "modern-javascript-es2023-features",
                                CategoryId = webDevCategory?.Id,
                                AuthorId = authorUser.Id,
                                Status = "Published",
                                PublishedAt = DateTime.UtcNow.AddDays(-5),
                                CreatedAt = DateTime.UtcNow.AddDays(-7),
                                UpdatedAt = DateTime.UtcNow.AddDays(-5),
                                ViewCount = 634,
                                MetaTitle = "JavaScript ES2023 Features - New Array Methods & More",
                                MetaDescription = "Comprehensive guide to ES2023 JavaScript features including findLast, toReversed, toSorted, and other modern improvements."
                            },
                            new Post
                            {
                                Title = "Docker for .NET Developers: Complete Guide",
                                Content = GetDockerArticleContent(),
                                Excerpt = "Complete guide to Docker containerization for .NET developers, covering Dockerfile creation, multi-stage builds, and production deployment strategies.",
                                Slug = "docker-dotnet-developers-complete-guide",
                                CategoryId = devopsCategory?.Id,
                                AuthorId = editorUser.Id,
                                Status = "Published",
                                PublishedAt = DateTime.UtcNow.AddDays(-3),
                                CreatedAt = DateTime.UtcNow.AddDays(-5),
                                UpdatedAt = DateTime.UtcNow.AddDays(-3),
                                ViewCount = 456,
                                MetaTitle = "Docker for .NET Developers - Complete Containerization Guide",
                                MetaDescription = "Learn Docker containerization for .NET applications with practical examples, best practices, and production deployment strategies."
                            }
                        };

                        await context.Posts.AddRangeAsync(posts);
                        await context.SaveChangesAsync();

                        // Add post-tag relationships
                        await SeedPostTagsAsync(context, logger);

                        logger.LogInformation("Posts seeded successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding posts");
            }
        }

        private static async Task SeedPostTagsAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.PostTags.AnyAsync())
                {
                    var posts = await context.Posts.ToListAsync();
                    var tags = await context.Tags.ToListAsync();

                    var postTagMappings = new List<(string postSlug, string[] tagSlugs)>
                    {
                        ("getting-started-aspnet-core-mvc", new[] { "aspnet-core", "csharp", "web-development", "tutorial" }),
                        ("entity-framework-core-best-practices", new[] { "csharp", "entity-framework", "sql-server", "performance" }),
                        ("modern-javascript-es2023-features", new[] { "javascript", "web-development", "tutorial" }),
                        ("docker-dotnet-developers-complete-guide", new[] { "csharp", "docker", "azure", "tutorial" })
                    };

                    var postTags = new List<PostTag>();

                    foreach (var mapping in postTagMappings)
                    {
                        var post = posts.FirstOrDefault(p => p.Slug == mapping.postSlug);
                        if (post != null)
                        {
                            foreach (var tagSlug in mapping.tagSlugs)
                            {
                                var tag = tags.FirstOrDefault(t => t.Slug == tagSlug);
                                if (tag != null)
                                {
                                    postTags.Add(new PostTag { PostId = post.Id, TagId = tag.Id });
                                }
                            }
                        }
                    }

                    if (postTags.Any())
                    {
                        await context.PostTags.AddRangeAsync(postTags);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding post tags");
            }
        }

        private static async Task SeedCommentsAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.Comments.AnyAsync())
                {
                    var posts = await context.Posts.ToListAsync();
                    var mvcPost = posts.FirstOrDefault(p => p.Slug == "getting-started-aspnet-core-mvc");
                    var efPost = posts.FirstOrDefault(p => p.Slug == "entity-framework-core-best-practices");
                    var jsPost = posts.FirstOrDefault(p => p.Slug == "modern-javascript-es2023-features");
                    var dockerPost = posts.FirstOrDefault(p => p.Slug == "docker-dotnet-developers-complete-guide");

                    var comments = new List<Comment>();

                    if (mvcPost != null)
                    {
                        comments.AddRange(new[]
                        {
                            new Comment { PostId = mvcPost.Id, AuthorName = "Mike Johnson", AuthorEmail = "mike.johnson@example.com", Content = "Great tutorial! This really helped me understand the MVC pattern better. Looking forward to the next article about Entity Framework integration.", IsApproved = true, CreatedAt = DateTime.UtcNow.AddDays(-14), UpdatedAt = DateTime.UtcNow.AddDays(-14), IpAddress = "192.168.1.100" },
                            new Comment { PostId = mvcPost.Id, AuthorName = "Sarah Wilson", AuthorEmail = "sarah.wilson@example.com", Content = "The project structure explanation was very clear. Could you add more information about routing in the next part?", IsApproved = true, CreatedAt = DateTime.UtcNow.AddDays(-13), UpdatedAt = DateTime.UtcNow.AddDays(-13), IpAddress = "10.0.0.50" }
                        });
                    }

                    if (efPost != null)
                    {
                        comments.Add(new Comment { PostId = efPost.Id, AuthorName = "David Chen", AuthorEmail = "david.chen@example.com", Content = "Excellent best practices! I've been making some of these mistakes in my projects. The repository pattern example is particularly helpful.", IsApproved = true, CreatedAt = DateTime.UtcNow.AddDays(-9), UpdatedAt = DateTime.UtcNow.AddDays(-9), IpAddress = "172.16.0.25" });
                    }

                    if (jsPost != null)
                    {
                        comments.Add(new Comment { PostId = jsPost.Id, AuthorName = "Tom Anderson", AuthorEmail = "tom.anderson@example.com", Content = "The new array methods are game-changers! findLast() is going to save me so much time. Thanks for the practical examples.", IsApproved = true, CreatedAt = DateTime.UtcNow.AddDays(-4), UpdatedAt = DateTime.UtcNow.AddDays(-4), IpAddress = "203.0.113.10" });
                    }

                    if (dockerPost != null)
                    {
                        comments.Add(new Comment { PostId = dockerPost.Id, AuthorName = "Emily Rodriguez", AuthorEmail = "emily.rodriguez@example.com", Content = "This Docker guide is comprehensive! The multi-stage build explanation finally made it click for me. Great work on the production considerations section.", IsApproved = true, CreatedAt = DateTime.UtcNow.AddDays(-2), UpdatedAt = DateTime.UtcNow.AddDays(-2), IpAddress = "198.51.100.5" });
                    }

                    if (comments.Any())
                    {
                        await context.Comments.AddRangeAsync(comments);
                        await context.SaveChangesAsync();
                        logger.LogInformation("Comments seeded successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding comments");
            }
        }

        private static string GetMvcArticleContent()
        {
            return @"ASP.NET Core MVC is a powerful framework for building modern web applications. In this comprehensive guide, we'll explore the fundamentals of MVC architecture and walk through creating your first application.

## What is MVC?

Model-View-Controller (MVC) is a software architectural pattern that separates an application into three main logical components:

- **Model**: Represents the data and business logic
- **View**: Handles the display of information
- **Controller**: Acts as an interface between Model and View components

## Setting up your first project

To get started, you'll need the .NET SDK installed on your machine. Once you have that ready, open your terminal and run:

```bash
dotnet new mvc -n MyFirstMvcApp
cd MyFirstMvcApp
dotnet run
```

This will create a new MVC project and start the development server. Navigate to `https://localhost:5001` to see your application running.

## Understanding the project structure

Let's explore the key folders and files in your new project:

- **Controllers/**: Contains your controller classes
- **Views/**: Contains your Razor view templates
- **Models/**: Contains your data models
- **wwwroot/**: Contains static files (CSS, JS, images)
- **Program.cs**: Application entry point and configuration

## Creating your first controller

Controllers handle incoming HTTP requests and return responses. Here's a simple example:

```csharp
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
```

## Next steps

In upcoming articles, we'll dive deeper into:
- Working with models and data
- Creating dynamic views with Razor
- Handling forms and user input
- Database integration with Entity Framework Core

Happy coding!";
        }

        private static string GetEfCoreArticleContent()
        {
            return @"Entity Framework Core is Microsoft's modern object-database mapper for .NET applications. Following best practices ensures optimal performance and maintainable code.

## 1. Use Async Methods

Always use async methods when interacting with the database to avoid blocking threads:

```csharp
var users = await context.Users.ToListAsync();
var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
```

## 2. Implement Repository Pattern

The repository pattern helps abstract data access logic:

```csharp
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}
```

## 3. Use Proper Indexing

Create indexes for frequently queried columns:

```csharp
modelBuilder.Entity<User>()
    .HasIndex(u => u.Email)
    .IsUnique();
```

## 4. Implement Soft Delete

Instead of hard deleting records, use soft delete:

```csharp
public class BaseEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
```

## Conclusion

Following these best practices will help you build robust, performant applications with Entity Framework Core. Remember to always profile your queries and optimize where necessary.";
        }

        private static string GetJavaScriptArticleContent()
        {
            return @"JavaScript continues to evolve with exciting new features. Let's explore the latest additions in ES2023 that can improve your development experience.

## Array Methods

### Array.prototype.findLast() and findLastIndex()

These methods allow you to search arrays from the end:

```javascript
const numbers = [1, 2, 3, 4, 5, 4, 3, 2, 1];
const lastEven = numbers.findLast(n => n % 2 === 0);
const lastEvenIndex = numbers.findLastIndex(n => n % 2 === 0);

console.log(lastEven); // 2
console.log(lastEvenIndex); // 7
```

## Browser Support

Check compatibility before using these features in production. Most modern browsers support them, but consider polyfills for older environments.

Stay tuned for more JavaScript updates!";
        }

        private static string GetDockerArticleContent()
        {
            return @"Docker has revolutionized application deployment by providing consistent environments across development, testing, and production. This guide covers everything .NET developers need to know about Docker.

## What is Docker?

Docker is a containerization platform that packages applications and their dependencies into lightweight, portable containers.

## Creating a Dockerfile for .NET

Here's a multi-stage Dockerfile for a .NET 8 application:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy source code and build
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT [""dotnet"", ""YourApp.dll""]
```

## Conclusion

Docker provides powerful capabilities for .NET applications. Start with simple containers and gradually adopt advanced patterns as your needs grow.";
        }
    }
}