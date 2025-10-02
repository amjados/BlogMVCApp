using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using BlogMVCApp.Configuration;
using System.Collections.Concurrent;

namespace BlogMVCApp.Filters;

/// <summary>
/// Configuration-based rate limiting filter
/// </summary>
public class ConfigurableRateLimitAttribute : ActionFilterAttribute
{
    private static readonly ConcurrentDictionary<string, List<DateTime>> _requestTracker = new();
    private readonly string? _actionOverride;

    public ConfigurableRateLimitAttribute(string? actionOverride = null)
    {
        _actionOverride = actionOverride;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var filterConfig = context.HttpContext.RequestServices.GetRequiredService<IOptions<FilterConfiguration>>().Value;
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ConfigurableRateLimitAttribute>>();

        if (!filterConfig.RateLimiting.Default.MaxRequests.HasValue())
        {
            base.OnActionExecuting(context);
            return;
        }

        var correlationId = context.HttpContext.TraceIdentifier;
        var actionName = _actionOverride ?? context.ActionDescriptor.RouteValues["action"] ?? "Unknown";

        // Get configuration for this action
        var rateLimitConfig = filterConfig.RateLimiting.Actions.ContainsKey(actionName)
            ? filterConfig.RateLimiting.Actions[actionName]
            : new ActionRateLimit
            {
                MaxRequests = filterConfig.RateLimiting.Default.MaxRequests,
                TimeWindowMinutes = filterConfig.RateLimiting.Default.TimeWindowMinutes,
                PerUser = filterConfig.RateLimiting.Default.PerUser
            };

        // Create tracking key
        var key = rateLimitConfig.PerUser && context.HttpContext.User.Identity?.IsAuthenticated == true
            ? $"user_{context.HttpContext.User.Identity.Name}_{actionName}"
            : $"ip_{context.HttpContext.Connection.RemoteIpAddress}_{actionName}";

        var now = DateTime.UtcNow;
        var timeWindow = TimeSpan.FromMinutes(rateLimitConfig.TimeWindowMinutes);
        var requests = _requestTracker.GetOrAdd(key, _ => new List<DateTime>());

        lock (requests)
        {
            // Remove old requests outside the time window
            requests.RemoveAll(r => now - r > timeWindow);

            // Check if limit exceeded
            if (requests.Count >= rateLimitConfig.MaxRequests)
            {
                var nextAvailableTime = requests.First().Add(timeWindow);
                var retryAfter = (int)(nextAvailableTime - now).TotalSeconds;

                logger.LogWarning("🚨 CONFIGURABLE RATE LIMIT exceeded | Action: {Action} | Config: {MaxRequests}/{TimeWindow}min | Key: {Key} | CorrelationId: {CorrelationId}",
                    actionName,
                    rateLimitConfig.MaxRequests,
                    rateLimitConfig.TimeWindowMinutes,
                    key.Split('_')[0] + "_***",
                    correlationId);

                HandleRateLimitExceeded(context, retryAfter, rateLimitConfig.MaxRequests, timeWindow, correlationId);
                return;
            }

            // Add current request
            requests.Add(now);

            // Add rate limit headers
            context.HttpContext.Response.Headers["X-RateLimit-Limit"] = rateLimitConfig.MaxRequests.ToString();
            context.HttpContext.Response.Headers["X-RateLimit-Remaining"] = (rateLimitConfig.MaxRequests - requests.Count).ToString();
            context.HttpContext.Response.Headers["X-RateLimit-Reset"] = ((DateTimeOffset)now.Add(timeWindow)).ToUnixTimeSeconds().ToString();

            logger.LogDebug("📊 CONFIGURABLE RATE LIMIT | Action: {Action} | Requests: {Count}/{Max} | Config-Based",
                actionName,
                requests.Count,
                rateLimitConfig.MaxRequests);
        }

        base.OnActionExecuting(context);
    }

    private static void HandleRateLimitExceeded(ActionExecutingContext context, int retryAfter, int maxRequests, TimeSpan timeWindow, string correlationId)
    {
        var acceptHeader = context.HttpContext.Request.Headers["Accept"].ToString();
        var isApiRequest = acceptHeader.Contains("application/json", StringComparison.OrdinalIgnoreCase) ||
                          context.HttpContext.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);

        if (isApiRequest)
        {
            var errorResponse = new
            {
                Success = false,
                Message = "Rate limit exceeded. Please try again later.",
                CorrelationId = correlationId,
                RetryAfter = retryAfter,
                Limit = maxRequests,
                Window = timeWindow.ToString(),
                Source = "Configuration"
            };

            context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(errorResponse) { StatusCode = 429 };
        }
        else
        {
            context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();
            context.Result = new Microsoft.AspNetCore.Mvc.ViewResult
            {
                ViewName = "RateLimitExceeded",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["RetryAfter"] = retryAfter,
                    ["MaxRequests"] = maxRequests,
                    ["TimeWindow"] = timeWindow.ToString(),
                    ["Source"] = "Configuration-Based"
                },
                StatusCode = 429
            };
        }
    }

    // Cleanup method to remove old entries
    public static void Cleanup()
    {
        var cutoff = DateTime.UtcNow.AddHours(-1);
        var keysToRemove = new List<string>();

        foreach (var kvp in _requestTracker)
        {
            lock (kvp.Value)
            {
                kvp.Value.RemoveAll(r => r < cutoff);
                if (kvp.Value.Count == 0)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }
        }

        foreach (var key in keysToRemove)
        {
            _requestTracker.TryRemove(key, out _);
        }
    }
}

// Extension method to help with nullable checks
public static class ConfigurationExtensions
{
    public static bool HasValue(this int value) => value > 0;
}