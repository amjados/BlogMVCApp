using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BlogMVCApp.Filters;

/// <summary>
/// Action filter that provides response caching for specific actions
/// </summary>
public class CacheResponseAttribute : ActionFilterAttribute
{
    private readonly int _durationSeconds;
    private readonly string? _cacheKeyPrefix;
    private readonly bool _varyByUser;
    private readonly bool _varyByQueryString;

    public CacheResponseAttribute(int durationSeconds = 300, string? cacheKeyPrefix = null, bool varyByUser = false, bool varyByQueryString = true)
    {
        _durationSeconds = durationSeconds;
        _cacheKeyPrefix = cacheKeyPrefix;
        _varyByUser = varyByUser;
        _varyByQueryString = varyByQueryString;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CacheResponseAttribute>>();

        // Generate cache key
        var cacheKey = GenerateCacheKey(context);

        // Check if response is cached
        if (memoryCache.TryGetValue(cacheKey, out var cachedResponse) && cachedResponse is CachedActionResult cached)
        {
            logger.LogInformation("💾 CACHE HIT: {CacheKey} | Action: {Action} | Age: {Age}s",
                cacheKey,
                context.ActionDescriptor.DisplayName,
                (DateTime.UtcNow - cached.CachedAt).TotalSeconds);

            // Set cache headers
            context.HttpContext.Response.Headers["X-Cache"] = "HIT";
            context.HttpContext.Response.Headers["X-Cache-Key"] = cacheKey;
            context.HttpContext.Response.Headers["Age"] = ((int)(DateTime.UtcNow - cached.CachedAt).TotalSeconds).ToString();

            context.Result = cached.Result;
            return;
        }

        // Execute action
        var executedContext = await next();

        // Cache successful results only
        if (executedContext.Exception == null && executedContext.Result != null)
        {
            var shouldCache = ShouldCacheResult(executedContext);

            if (shouldCache)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_durationSeconds),
                    Size = 1,
                    Priority = CacheItemPriority.Normal
                };

                var cacheEntry = new CachedActionResult
                {
                    Result = executedContext.Result,
                    CachedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(_durationSeconds)
                };

                memoryCache.Set(cacheKey, cacheEntry, cacheOptions);

                logger.LogInformation("💾 CACHE STORED: {CacheKey} | Action: {Action} | Duration: {Duration}s | Size: ~{Size} bytes",
                    cacheKey,
                    context.ActionDescriptor.DisplayName,
                    _durationSeconds,
                    EstimateResultSize(executedContext.Result));

                // Set cache headers
                context.HttpContext.Response.Headers["X-Cache"] = "MISS";
                context.HttpContext.Response.Headers["X-Cache-Key"] = cacheKey;
                context.HttpContext.Response.Headers["Cache-Control"] = $"public, max-age={_durationSeconds}";
                context.HttpContext.Response.Headers["Expires"] = cacheEntry.ExpiresAt.ToString("R");
            }
            else
            {
                context.HttpContext.Response.Headers["X-Cache"] = "SKIP";
                logger.LogDebug("💾 CACHE SKIPPED: {CacheKey} | Reason: Result not cacheable", cacheKey);
            }
        }
    }

    private string GenerateCacheKey(ActionExecutingContext context)
    {
        var keyParts = new List<string>();

        // Base key
        var baseKey = _cacheKeyPrefix ?? "ActionCache";
        keyParts.Add(baseKey);

        // Controller and Action
        keyParts.Add(context.RouteData.Values["controller"]?.ToString() ?? "UnknownController");
        keyParts.Add(context.RouteData.Values["action"]?.ToString() ?? "UnknownAction");

        // User variation
        if (_varyByUser && context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            keyParts.Add($"user_{context.HttpContext.User.Identity.Name}");
        }

        // Query string variation
        if (_varyByQueryString && context.HttpContext.Request.QueryString.HasValue)
        {
            var queryString = context.HttpContext.Request.QueryString.Value;
            keyParts.Add($"query_{queryString.GetHashCode()}");
        }

        // Route parameters
        foreach (var param in context.ActionArguments)
        {
            if (param.Value != null)
            {
                keyParts.Add($"{param.Key}_{param.Value}");
            }
        }

        return string.Join("_", keyParts);
    }

    private static bool ShouldCacheResult(ActionExecutedContext context)
    {
        // Only cache successful results
        if (context.HttpContext.Response.StatusCode < 200 || context.HttpContext.Response.StatusCode >= 400)
            return false;

        // Don't cache if there are model errors
        if (!context.ModelState.IsValid)
            return false;

        // Check result types that can be cached
        return context.Result switch
        {
            ViewResult => true,
            JsonResult => true,
            ContentResult => true,
            FileResult => false, // Files should be cached differently
            RedirectResult => false, // Don't cache redirects
            _ => false
        };
    }

    private static long EstimateResultSize(IActionResult result)
    {
        return result switch
        {
            JsonResult jsonResult => EstimateJsonSize(jsonResult.Value),
            ContentResult contentResult => contentResult.Content?.Length ?? 0,
            ViewResult => 5000, // Estimate for typical view
            _ => 1000
        };
    }

    private static long EstimateJsonSize(object? value)
    {
        if (value == null) return 0;

        try
        {
            var json = JsonSerializer.Serialize(value);
            return json.Length;
        }
        catch
        {
            return 1000; // Fallback estimate
        }
    }

    private class CachedActionResult
    {
        public required IActionResult Result { get; set; }
        public DateTime CachedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}