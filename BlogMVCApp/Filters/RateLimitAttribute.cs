using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;

namespace BlogMVCApp.Filters;

/// <summary>
/// Action filter that implements rate limiting per action/user
/// </summary>
public class RateLimitAttribute : ActionFilterAttribute
{
    private static readonly ConcurrentDictionary<string, List<DateTime>> _requestTracker = new();
    private readonly int _maxRequests;
    private readonly TimeSpan _timeWindow;
    private readonly bool _perUser;

    public RateLimitAttribute(int maxRequests = 10, int timeWindowMinutes = 1, bool perUser = true)
    {
        _maxRequests = maxRequests;
        _timeWindow = TimeSpan.FromMinutes(timeWindowMinutes);
        _perUser = perUser;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<RateLimitAttribute>>();
        var correlationId = context.HttpContext.TraceIdentifier;

        // Create tracking key
        var key = _perUser && context.HttpContext.User.Identity?.IsAuthenticated == true
            ? $"user_{context.HttpContext.User.Identity.Name}_{context.ActionDescriptor.DisplayName}"
            : $"ip_{context.HttpContext.Connection.RemoteIpAddress}_{context.ActionDescriptor.DisplayName}";

        var now = DateTime.UtcNow;
        var requests = _requestTracker.GetOrAdd(key, _ => new List<DateTime>());

        lock (requests)
        {
            // Remove old requests outside the time window
            requests.RemoveAll(r => now - r > _timeWindow);

            // Check if limit exceeded
            if (requests.Count >= _maxRequests)
            {
                var nextAvailableTime = requests.First().Add(_timeWindow);
                var retryAfter = (int)(nextAvailableTime - now).TotalSeconds;

                logger.LogWarning("🚨 RATE LIMIT exceeded for action | Key: {Key} | Requests: {RequestCount}/{MaxRequests} | Window: {TimeWindow} | CorrelationId: {CorrelationId}",
                    key.Split('_')[0] + "_***", // Mask the actual key for privacy
                    requests.Count,
                    _maxRequests,
                    _timeWindow,
                    correlationId);

                // Check if this is an API request
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
                        Limit = _maxRequests,
                        Window = _timeWindow.ToString()
                    };

                    context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();
                    context.HttpContext.Response.Headers["X-RateLimit-Limit"] = _maxRequests.ToString();
                    context.HttpContext.Response.Headers["X-RateLimit-Remaining"] = "0";
                    context.HttpContext.Response.Headers["X-RateLimit-Reset"] = ((DateTimeOffset)nextAvailableTime).ToUnixTimeSeconds().ToString();

                    context.Result = new JsonResult(errorResponse) { StatusCode = 429 };
                }
                else
                {
                    // For web requests, show a friendly error page
                    context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();
                    context.Result = new ViewResult
                    {
                        ViewName = "RateLimitExceeded",
                        ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                            new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                            new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                        {
                            ["RetryAfter"] = retryAfter,
                            ["MaxRequests"] = _maxRequests,
                            ["TimeWindow"] = _timeWindow.ToString()
                        },
                        StatusCode = 429
                    };
                }

                return;
            }

            // Add current request
            requests.Add(now);

            // Add rate limit headers for API responses
            context.HttpContext.Response.Headers["X-RateLimit-Limit"] = _maxRequests.ToString();
            context.HttpContext.Response.Headers["X-RateLimit-Remaining"] = (_maxRequests - requests.Count).ToString();
            context.HttpContext.Response.Headers["X-RateLimit-Reset"] = ((DateTimeOffset)now.Add(_timeWindow)).ToUnixTimeSeconds().ToString();

            logger.LogDebug("📊 RATE LIMIT status | Key: {Key} | Requests: {RequestCount}/{MaxRequests} | Remaining: {Remaining}",
                key.Split('_')[0] + "_***",
                requests.Count,
                _maxRequests,
                _maxRequests - requests.Count);
        }

        base.OnActionExecuting(context);
    }

    // Cleanup method to remove old entries (should be called periodically)
    public static void Cleanup()
    {
        var cutoff = DateTime.UtcNow.AddHours(-1); // Remove entries older than 1 hour
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