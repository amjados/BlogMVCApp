using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogMVCApp.Models;
using BlogMVCApp.Services;

namespace BlogMVCApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BlogController> _logger;

        public BlogController(IBlogService blogService, UserManager<ApplicationUser> userManager, ILogger<BlogController> logger)
        {
            _blogService = blogService;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Blog/Post/{slug}
        [Route("Blog/Post/{slug}")]
        public async Task<IActionResult> Post(string slug)
        {
            try
            {
                var post = await _blogService.GetPostBySlugAsync(slug);
                if (post == null || post.Status != "Published")
                {
                    return NotFound();
                }

                // Increment view count (this will invalidate the cache)
                await _blogService.IncrementPostViewCountAsync(post.Id);

                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading post with slug: {Slug}", slug);
                return NotFound();
            }
        }

        // GET: /Blog/Category/{slug}
        [Route("Blog/Category/{slug}")]
        public async Task<IActionResult> Category(string slug, int page = 1)
        {
            try
            {
                var category = await _blogService.GetCategoryBySlugAsync(slug);
                if (category == null)
                {
                    return NotFound();
                }

                // For now, we'll implement a simple category view
                // In a full implementation, you'd get posts by category
                ViewBag.Category = category;
                ViewBag.Page = page;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading category with slug: {Slug}", slug);
                return NotFound();
            }
        }

        // GET: /Blog/Search
        public IActionResult Search(string query, int page = 1)
        {
            try
            {
                // For now, return empty results
                // In a full implementation, you'd implement search functionality
                ViewBag.Query = query ?? string.Empty;
                ViewBag.Page = page;
                ViewBag.Posts = new List<Post>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing search with query: {Query}", query);
                ViewBag.Query = query ?? string.Empty;
                ViewBag.Posts = new List<Post>();
                return View();
            }
        }

        // POST: /Blog/Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(int postId, string content, string authorName, string authorEmail, int? parentCommentId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(authorName))
                {
                    TempData["CommentError"] = "Name and comment are required.";
                    return RedirectToAction("Post", new { id = postId });
                }

                var currentUser = await _userManager.GetUserAsync(User);

                var comment = new Comment
                {
                    PostId = postId,
                    AuthorId = currentUser?.Id,
                    AuthorName = currentUser?.FullName ?? authorName.Trim(),
                    AuthorEmail = currentUser?.Email ?? authorEmail?.Trim(),
                    Content = content.Trim(),
                    ParentCommentId = parentCommentId,
                    IsApproved = currentUser != null, // Auto-approve for logged-in users
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };

                await _blogService.CreateCommentAsync(comment);

                var message = comment.IsApproved
                    ? "Your comment has been posted!"
                    : "Your comment has been submitted and is awaiting approval.";

                TempData["CommentSuccess"] = message;

                return RedirectToAction("Post", new { id = postId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting comment for post ID: {PostId}", postId);
                TempData["CommentError"] = "An error occurred while posting your comment. Please try again.";
                return RedirectToAction("Post", new { id = postId });
            }
        }

        // Demo action with explicit attributes for educational purposes
        [Route("Blog/TestBlogCaching")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByHeader = "User-Agent")]
        public IActionResult TestBlogCaching()
        {
            ViewBag.Message = "This is a demo action showing explicit caching attributes";
            ViewBag.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View("TestDemo");
        }
    }
}