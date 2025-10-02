using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using BlogMVCApp.Configuration;
using System.Text.Json;

namespace BlogMVCApp.Filters;

/// <summary>
/// Configuration-based response caching filter
/// </summary>
public class ConfigurableCacheResponseAttribute : ActionFilterAttribute
{
    private readonly string? _actionOverride;

    public ConfigurableCacheResponseAttribute(string? actionOverride = null)
    {
        _actionOverride = actionOverride;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var filterConfig = context.HttpContext.RequestServices.GetRequiredService<IOptions<FilterConfiguration>>().Value;
        var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ConfigurableCacheResponseAttribute>>();

        var actionName = _actionOverride ?? context.ActionDescriptor.RouteValues["action"] ?? "Unknown";

        // Get configuration for this action
        var cacheConfig = filterConfig.Caching.Actions.ContainsKey(actionName)
            ? filterConfig.Caching.Actions[actionName]
            : new ActionCache
            {
                DurationSeconds = filterConfig.Caching.Default.DurationSeconds,
                VaryByUser = filterConfig.Caching.Default.VaryByUser,
                VaryByQueryString = filterConfig.Caching.Default.VaryByQueryString
            };

        // Skip caching if duration is 0 or negative
        if (cacheConfig.DurationSeconds <= 0)
        {
            logger.LogDebug("🚫 CACHING DISABLED for action {Action} (duration: {Duration})", actionName, cacheConfig.DurationSeconds);
            await next();
            return;
        }

        // Generate cache key
        var cacheKey = GenerateCacheKey(context, actionName, cacheConfig);

        // Check if response is cached
        if (memoryCache.TryGetValue(cacheKey, out var cachedResponse) && cachedResponse is CachedActionResult cached)
        {
            logger.LogInformation("💾 CONFIG CACHE HIT: {Action} | Key: {CacheKey} | Age: {Age}s | Config: {Duration}s",
                actionName,
                cacheKey,
                (DateTime.UtcNow - cached.CachedAt).TotalSeconds,
                cacheConfig.DurationSeconds);

            // Set cache headers
            context.HttpContext.Response.Headers["X-Cache"] = "HIT";
            context.HttpContext.Response.Headers["X-Cache-Key"] = cacheKey;
            context.HttpContext.Response.Headers["X-Cache-Source"] = "Configuration";
            context.HttpContext.Response.Headers["Age"] = ((int)(DateTime.UtcNow - cached.CachedAt).TotalSeconds).ToString();

            context.Result = cached.Result;
            return;
        }

        // Execute action
        var executedContext = await next();

        // Cache successful results only
        if (executedContext.Exception == null && executedContext.Result != null && ShouldCacheResult(executedContext))
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheConfig.DurationSeconds),
                Size = 1,
                Priority = CacheItemPriority.Normal
            };

            var cacheEntry = new CachedActionResult
            {
                Result = executedContext.Result,
                CachedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddSeconds(cacheConfig.DurationSeconds)
            };

            memoryCache.Set(cacheKey, cacheEntry, cacheOptions);

            logger.LogInformation("💾 CONFIG CACHE STORED: {Action} | Key: {CacheKey} | Duration: {Duration}s | Config-Based",
                actionName,
                cacheKey,
                cacheConfig.DurationSeconds);

            // Set cache headers
            context.HttpContext.Response.Headers["X-Cache"] = "MISS";
            context.HttpContext.Response.Headers["X-Cache-Key"] = cacheKey;
            context.HttpContext.Response.Headers["X-Cache-Source"] = "Configuration";
            context.HttpContext.Response.Headers["Cache-Control"] = $"public, max-age={cacheConfig.DurationSeconds}";
            context.HttpContext.Response.Headers["Expires"] = cacheEntry.ExpiresAt.ToString("R");
        }
        else
        {
            context.HttpContext.Response.Headers["X-Cache"] = "SKIP";
            logger.LogDebug("💾 CONFIG CACHE SKIPPED: {Action} | Reason: Result not cacheable", actionName);
        }
    }

    private string GenerateCacheKey(ActionExecutingContext context, string actionName, ActionCache config)
    {
        var keyParts = new List<string> { "ConfigCache", actionName };

        // Controller name
        keyParts.Add(context.RouteData.Values["controller"]?.ToString() ?? "UnknownController");

        // User variation
        if (config.VaryByUser && context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            keyParts.Add($"user_{context.HttpContext.User.Identity.Name}");
        }

        // Query string variation
        if (config.VaryByQueryString && context.HttpContext.Request.QueryString.HasValue)
        {
            var queryString = context.HttpContext.Request.QueryString.Value;
            keyParts.Add($"query_{queryString!.GetHashCode()}");
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
        if (context.HttpContext.Response.StatusCode < 200 || context.HttpContext.Response.StatusCode >= 400)
            return false;

        if (!context.ModelState.IsValid)
            return false;

        return context.Result switch
        {
            ViewResult => true,
            JsonResult => true,
            ContentResult => true,
            FileResult => false,
            RedirectResult => false,
            _ => false
        };
    }

    private class CachedActionResult
    {
        public required IActionResult Result { get; set; }
        public DateTime CachedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}