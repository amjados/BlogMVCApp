using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogMVCApp.Services;

namespace BlogMVCApp.Controllers
{
    public class CacheController : Controller
    {
        private readonly ICacheService _cacheService;
        private readonly ICacheWarmupService _cacheWarmupService;
        private readonly ILogger<CacheController> _logger;

        public CacheController(ICacheService cacheService, ICacheWarmupService cacheWarmupService, ILogger<CacheController> logger)
        {
            _cacheService = cacheService;
            _cacheWarmupService = cacheWarmupService;
            _logger = logger;
        }

        // GET: /Cache
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Cache/ClearAll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearAll()
        {
            try
            {
                // Clear all caches by removing common patterns
                await _cacheService.RemovePatternAsync("post:");
                await _cacheService.RemovePatternAsync("posts:");
                await _cacheService.RemovePatternAsync("category:");
                await _cacheService.RemovePatternAsync("categories:");
                await _cacheService.RemovePatternAsync("comments:");
                await _cacheService.RemovePatternAsync("tags:");
                await _cacheService.RemovePatternAsync("stats:");

                TempData["Success"] = "All caches have been cleared successfully.";
                _logger.LogInformation("All caches cleared by user {User}", User.Identity?.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing all caches");
                TempData["Error"] = "An error occurred while clearing caches.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cache/ClearPosts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearPosts()
        {
            try
            {
                await _cacheService.RemovePatternAsync("post:");
                await _cacheService.RemovePatternAsync("posts:");

                TempData["Success"] = "Post caches have been cleared successfully.";
                _logger.LogInformation("Post caches cleared by user {User}", User.Identity?.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing post caches");
                TempData["Error"] = "An error occurred while clearing post caches.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cache/ClearCategories
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCategories()
        {
            try
            {
                await _cacheService.RemovePatternAsync("category:");
                await _cacheService.RemovePatternAsync("categories:");

                TempData["Success"] = "Category caches have been cleared successfully.";
                _logger.LogInformation("Category caches cleared by user {User}", User.Identity?.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing category caches");
                TempData["Error"] = "An error occurred while clearing category caches.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cache/Warmup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Warmup()
        {
            try
            {
                await _cacheWarmupService.WarmupCacheAsync();

                TempData["Success"] = "Cache warmup completed successfully.";
                _logger.LogInformation("Cache warmed up by user {User}", User.Identity?.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cache warmup");
                TempData["Error"] = "An error occurred during cache warmup.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Cache/Stats
        public IActionResult Stats()
        {
            try
            {
                // In a real implementation, you might want to track cache statistics
                // For now, we'll return a simple view
                ViewBag.Message = "Cache statistics would be displayed here.";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cache stats");
                TempData["Error"] = "An error occurred while retrieving cache statistics.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Demo action with explicit attributes for educational purposes
        [Authorize]
        public IActionResult TestCacheControllerAuth()
        {
            ViewBag.Message = "This is a demo action showing explicit authorization attributes";
            ViewBag.UserName = User.Identity?.Name;
            ViewBag.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View("TestDemo");
        }
    }
}