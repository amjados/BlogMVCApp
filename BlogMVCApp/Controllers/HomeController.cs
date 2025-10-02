using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogMVCApp.Models;
using BlogMVCApp.Data;
using BlogMVCApp.Middleware;

namespace BlogMVCApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    [ResponseCache(Duration = 180, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Get published posts for public viewing
            var posts = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Where(p => p.Status == "Published")
                .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
                .Take(10)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Excerpt,
                    p.Slug,
                    p.FeaturedImageUrl,
                    p.PublishedAt,
                    p.ViewCount,
                    CategoryName = p.Category != null ? p.Category.Name : "Uncategorized",
                    CategorySlug = p.Category != null ? p.Category.Slug : null,
                    AuthorName = p.Author != null ? p.Author.FullName : "Anonymous"
                })
                .ToListAsync();

            ViewBag.Posts = posts;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading published posts");
            ViewBag.Posts = new List<object>();
            return View();
        }
    }

    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Login(string email, string password, bool rememberMe = false, string? returnUrl = null)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Error"] = "Email and password are required.";
            return View();
        }

        try
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["Error"] = "Invalid email or password.";
                return View();
            }

            // Attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {email} logged in successfully.");

                // Redirect to landing page after successful login
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("LandPage");
            }

            if (result.IsLockedOut)
            {
                ViewData["Error"] = "Account is locked out.";
            }
            else
            {
                ViewData["Error"] = "Invalid email or password.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login attempt for {Email}", email);
            ViewData["Error"] = "An error occurred during login. Please try again.";
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction("Index");
    }

    [Authorize]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public IActionResult LandPage()
    {
        // Check if user is admin and redirect to admin landing page
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("AdminLandPage");
        }

        return View();
    }

    [Authorize(Roles = "Admin")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> AdminLandPage()
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }

            // Get admin dashboard statistics
            var totalUsers = await _userManager.Users.CountAsync();
            var totalPosts = await _context.Posts.CountAsync();
            var totalPublishedPosts = await _context.Posts.CountAsync(p => p.Status == "Published");
            var totalCategories = await _context.Categories.CountAsync();
            var totalComments = await _context.Comments.CountAsync();

            // Recent activity - last 10 posts
            var recentPosts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Status,
                    p.CreatedAt,
                    AuthorName = p.Author != null ? p.Author.FullName : "Unknown",
                    CategoryName = p.Category != null ? p.Category.Name : "Uncategorized"
                })
                .ToListAsync();

            // Pass data to view
            ViewBag.AdminName = currentUser.FullName;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalPosts = totalPosts;
            ViewBag.TotalPublishedPosts = totalPublishedPosts;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalComments = totalComments;
            ViewBag.RecentPosts = recentPosts;

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
            TempData["Error"] = "An error occurred while loading the admin dashboard.";
            return View();
        }
    }

    [Authorize]
    [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Any, VaryByHeader = "User-Agent")]
    public async Task<IActionResult> WritePost()
    {
        // Load categories for the dropdown
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        ViewBag.Categories = categories;
        return View();
    }

    [Authorize]
    [ResponseCache(Duration = 900, Location = ResponseCacheLocation.Any, VaryByHeader = "User-Agent")]
    public async Task<IActionResult> ViewStats()
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }

            // Get statistics for the current user
            var totalPosts = await _context.Posts.CountAsync(p => p.AuthorId == currentUser.Id);
            var publishedPosts = await _context.Posts.CountAsync(p => p.AuthorId == currentUser.Id && p.Status == "Published");
            var draftPosts = await _context.Posts.CountAsync(p => p.AuthorId == currentUser.Id && p.Status == "Draft");
            var totalViews = await _context.Posts.Where(p => p.AuthorId == currentUser.Id).SumAsync(p => p.ViewCount);
            var totalComments = await _context.Comments
                .Where(c => c.Post != null && c.Post.AuthorId == currentUser.Id)
                .CountAsync();

            // Recent posts
            var recentPosts = await _context.Posts
                .Include(p => p.Category)
                .Where(p => p.AuthorId == currentUser.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Status,
                    p.CreatedAt,
                    p.ViewCount,
                    CategoryName = p.Category != null ? p.Category.Name : "Uncategorized"
                })
                .ToListAsync();

            // Pass data to view
            ViewBag.UserName = currentUser.FullName;
            ViewBag.UserEmail = currentUser.Email ?? "user@example.com";
            ViewBag.UserRole = User.IsInRole("Admin") ? "Administrator" :
                              User.IsInRole("Editor") ? "Editor" :
                              User.IsInRole("Author") ? "Author" : "User";

            ViewBag.TotalPosts = totalPosts;
            ViewBag.PublishedPosts = publishedPosts;
            ViewBag.DraftPosts = draftPosts;
            ViewBag.TotalViews = totalViews;
            ViewBag.TotalComments = totalComments;
            ViewBag.RecentPosts = recentPosts;

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading statistics");
            TempData["Error"] = "An error occurred while loading statistics.";
            return View();
        }
    }

    [Authorize]
    [HttpPost]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> SavePost(string title, string excerpt, string category, string tags,
        string content, string featuredImage, string status, DateTime? publishDate,
        string metaTitle, string metaDescription, string slug)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                TempData["Error"] = "User not found. Please log in again.";
                return RedirectToAction("Login");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Title and content are required.";
                return RedirectToAction("WritePost");
            }

            // Generate slug if not provided
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = GenerateSlug(title);
            }

            // Ensure slug is unique
            slug = await EnsureUniqueSlug(slug);

            // Parse category
            int? categoryId = null;
            if (!string.IsNullOrWhiteSpace(category) && int.TryParse(category, out var catId))
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == catId && c.IsActive);
                if (categoryExists)
                {
                    categoryId = catId;
                }
            }

            // Create the post
            var post = new Post
            {
                Title = title.Trim(),
                Content = content.Trim(),
                Excerpt = string.IsNullOrWhiteSpace(excerpt) ? null : excerpt.Trim(),
                Slug = slug,
                FeaturedImageUrl = string.IsNullOrWhiteSpace(featuredImage) ? null : featuredImage.Trim(),
                CategoryId = categoryId,
                AuthorId = currentUser.Id,
                Status = status?.ToLowerInvariant() == "published" ? "Published" : "Draft",
                PublishedAt = status?.ToLowerInvariant() == "published" ? DateTime.UtcNow : publishDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                MetaTitle = string.IsNullOrWhiteSpace(metaTitle) ? null : metaTitle.Trim(),
                MetaDescription = string.IsNullOrWhiteSpace(metaDescription) ? null : metaDescription.Trim(),
                ViewCount = 0
            };

            // Save the post
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Process tags if provided
            if (!string.IsNullOrWhiteSpace(tags))
            {
                await ProcessPostTags(post.Id, tags);
            }

            // Log the action
            await LogAuditAction("CreatePost", "Post", post.Id.ToString(), null,
                $"Created post: {post.Title}", currentUser.Id);

            var statusMessage = post.Status == "Published" ? "published" : "saved as draft";
            TempData["Success"] = $"Post '{title}' has been {statusMessage} successfully!";

            _logger.LogInformation($"Post '{title}' {statusMessage} by user {currentUser.Email}");

            return RedirectToAction("WritePost");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving post");
            TempData["Error"] = "An error occurred while saving the post. Please try again.";
            return RedirectToAction("WritePost");
        }
    }

    [Authorize]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "title", "content", "excerpt", "category" })]
    public IActionResult PreviewPost(string title, string content, string excerpt, string category)
    {
        // Pass data to preview view
        ViewBag.Title = title ?? "Untitled Post";
        ViewBag.Content = content ?? "No content available.";
        ViewBag.Excerpt = excerpt ?? "";
        ViewBag.Category = category ?? "Uncategorized";
        ViewBag.Author = User.Identity?.Name ?? "Anonymous";
        ViewBag.PublishDate = DateTime.Now;

        return View();
    }

    [Authorize]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, VaryByHeader = "User-Agent")]
    public async Task<IActionResult> ViewAllPosts()
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }

            // Get posts by current user, ordered by creation date
            var posts = await _context.Posts
                .Include(p => p.Category)
                .Where(p => p.AuthorId == currentUser.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Excerpt,
                    p.Status,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.PublishedAt,
                    p.ViewCount,
                    CategoryName = p.Category != null ? p.Category.Name : "Uncategorized",
                    p.Slug
                })
                .ToListAsync();

            ViewBag.Posts = posts;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading posts");
            TempData["Error"] = "An error occurred while loading posts.";
            return View();
        }
    }

    private async Task<string> EnsureUniqueSlug(string slug)
    {
        var originalSlug = slug;
        var counter = 1;

        while (await _context.Posts.AnyAsync(p => p.Slug == slug))
        {
            slug = $"{originalSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    private async Task ProcessPostTags(int postId, string tagsString)
    {
        if (string.IsNullOrWhiteSpace(tagsString))
            return;

        var tagNames = tagsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .Where(t => !string.IsNullOrEmpty(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var tagName in tagNames)
        {
            // Find or create tag
            var tagSlug = GenerateSlug(tagName);
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Slug == tagSlug);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName,
                    Slug = tagSlug,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            // Create post-tag relationship if it doesn't exist
            var postTagExists = await _context.PostTags
                .AnyAsync(pt => pt.PostId == postId && pt.TagId == tag.Id);

            if (!postTagExists)
            {
                _context.PostTags.Add(new PostTag
                {
                    PostId = postId,
                    TagId = tag.Id
                });
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task LogAuditAction(string action, string? entityType, string? entityId,
        string? oldValues, string? newValues, string userId)
    {
        try
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                OldValues = oldValues,
                NewValues = newValues,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString(),
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging audit action");
            // Don't throw - audit logging failures shouldn't break the main operation
        }
    }

    private string GenerateSlug(string title)
    {
        return title.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("'", "")
            .Replace("\"", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("?", "")
            .Replace("!", "")
            .Replace("&", "and")
            .Replace("@", "at")
            .Replace("$", "")
            .Replace("%", "")
            .Replace("^", "")
            .Replace("*", "")
            .Replace("(", "")
            .Replace(")", "");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string? correlationId = null, int? statusCode = null)
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            CorrelationId = correlationId ?? HttpContext.Items["ErrorCorrelationId"]?.ToString(),
            StatusCode = statusCode ?? HttpContext.Items["ErrorStatusCode"] as int? ?? 500,
            ErrorMessage = HttpContext.Items["ErrorMessage"]?.ToString() ?? "An unexpected error occurred"
        };

        _logger.LogInformation(
            "📄 Error page displayed: StatusCode: {StatusCode}, CorrelationId: {CorrelationId}, RequestId: {RequestId}",
            errorViewModel.StatusCode,
            errorViewModel.CorrelationId,
            errorViewModel.RequestId
        );

        return View(errorViewModel);
    }

    // Demo endpoints for testing middleware
    [Authorize(Roles = "Admin")]
    public IActionResult TestException()
    {
        throw new InvalidOperationException("This is a test exception to demonstrate global exception handling middleware.");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult TestNotFound()
    {
        throw new NotFoundException("This is a test NotFoundException to demonstrate custom exception handling.");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult TestValidation()
    {
        var errors = new List<string> { "Test validation error 1", "Test validation error 2" };
        throw new ValidationException(errors);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TestSlowRequest()
    {
        _logger.LogInformation("🐌 Starting intentionally slow request for testing");
        await Task.Delay(3000); // 3 second delay to trigger slow request logging
        return Json(new { message = "Slow request completed", duration = "3000ms" });
    }

    // ========== FILTER DEMONSTRATION ACTIONS ==========

    [BlogMVCApp.Filters.ValidateModel]
    [BlogMVCApp.Filters.LogAction("Testing model validation filter")]
    public IActionResult TestValidateModel(TestModel model)
    {
        // This will automatically validate the model using our ValidateModelAttribute
        return Json(new { success = true, message = "Model is valid!", data = model });
    }

    [BlogMVCApp.Filters.RateLimit(maxRequests: 3, timeWindowMinutes: 1, perUser: false)]
    [BlogMVCApp.Filters.LogAction("Testing rate limiting")]
    public IActionResult TestRateLimit()
    {
        return Json(new
        {
            success = true,
            message = "Rate limit test - try calling this endpoint more than 3 times in 1 minute",
            timestamp = DateTime.Now,
            tip = "Check the X-RateLimit-* headers in the response"
        });
    }

    [BlogMVCApp.Filters.CacheResponse(durationSeconds: 30, varyByUser: false)]
    [BlogMVCApp.Filters.LogAction("Testing response caching")]
    public IActionResult TestCaching()
    {
        return Json(new
        {
            success = true,
            message = "This response is cached for 30 seconds",
            timestamp = DateTime.Now,
            random = new Random().Next(1000, 9999),
            tip = "Refresh within 30 seconds to see cached response"
        });
    }

    [BlogMVCApp.Filters.CustomAuthorize(roles: "Admin,SuperUser", requireAllRoles: false)]
    [BlogMVCApp.Filters.LogAction("Testing custom authorization")]
    public IActionResult TestCustomAuth()
    {
        return Json(new
        {
            success = true,
            message = "Custom authorization passed! You have Admin or SuperUser role.",
            user = User.Identity?.Name,
            roles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value)
        });
    }

    [BlogMVCApp.Filters.CustomExceptionFilter(logException: true, customMessage: "Testing exception handling")]
    [BlogMVCApp.Filters.LogAction("Testing exception filter")]
    public IActionResult TestExceptionFilter(string? exceptionType = null)
    {
        // This demonstrates different exception types being handled
        switch (exceptionType?.ToLower())
        {
            case "argument":
                throw new ArgumentException("This is a test argument exception", "testParam");
            case "argumentnull":
                throw new ArgumentNullException("testParam", "This is a test null argument exception");
            case "invalidoperation":
                throw new InvalidOperationException("This is a test invalid operation exception");
            case "notimplemented":
                throw new NotImplementedException("This feature is not yet implemented (test)");
            case "timeout":
                throw new TimeoutException("This is a test timeout exception");
            default:
                throw new Exception("This is a test generic exception");
        }
    }

    [BlogMVCApp.Filters.LogAction("Combined filters test")]
    [BlogMVCApp.Filters.RateLimit(5, 1)]
    [BlogMVCApp.Filters.CacheResponse(60)]
    [BlogMVCApp.Filters.ValidateModel]
    public IActionResult TestCombinedFilters(TestModel model)
    {
        return Json(new
        {
            success = true,
            message = "All filters working together!",
            modelData = model,
            timestamp = DateTime.Now,
            features = new[] { "Rate Limiting", "Caching", "Model Validation", "Action Logging" }
        });
    }
}
