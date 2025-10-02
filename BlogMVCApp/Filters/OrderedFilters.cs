using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using BlogMVCApp.Configuration;

namespace BlogMVCApp.Filters;

/// <summary>
/// Base class for filters that support configuration-based ordering
/// </summary>
public abstract class OrderedFilterAttribute : Attribute, IOrderedFilter
{
    private readonly string _filterType;
    protected readonly string? _configKey;

    protected OrderedFilterAttribute(string filterType, string? configKey = null)
    {
        _filterType = filterType;
        _configKey = configKey;
    }

    public int Order { get; set; } = int.MaxValue;

    /// <summary>
    /// Gets the order from configuration or returns the default
    /// </summary>
    protected int GetConfiguredOrder(FilterContext context, int defaultOrder = int.MaxValue)
    {
        try
        {
            var filterManager = context.HttpContext.RequestServices.GetService<FilterManager>();
            if (filterManager != null)
            {
                var configuredOrder = filterManager.GetDefaultOrder(_filterType);
                if (configuredOrder < int.MaxValue)
                {
                    Order = configuredOrder;
                    return configuredOrder;
                }
            }
        }
        catch (Exception ex)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<OrderedFilterAttribute>>();
            logger?.LogWarning(ex, "Failed to get configured order for filter {FilterType}", _filterType);
        }

        Order = defaultOrder;
        return defaultOrder;
    }
}

/// <summary>
/// Ordered version of LogAction filter
/// </summary>
public class OrderedLogActionAttribute : OrderedFilterAttribute, IActionFilter
{
    private readonly string? _customMessage;

    public OrderedLogActionAttribute(string? customMessage = null, string? configKey = null)
        : base("LogAction", configKey)
    {
        _customMessage = customMessage;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        GetConfiguredOrder(context, 400); // Default order for logging

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OrderedLogActionAttribute>>();
        var correlationId = context.HttpContext.TraceIdentifier;

        logger.LogInformation("🎬 ORDERED ACTION STARTING: {Controller}.{Action} | Order: {Order} | CorrelationId: {CorrelationId}",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"],
            Order,
            correlationId);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OrderedLogActionAttribute>>();
        var correlationId = context.HttpContext.TraceIdentifier;

        logger.LogInformation("✅ ORDERED ACTION COMPLETED: {Controller}.{Action} | Order: {Order} | CorrelationId: {CorrelationId}",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"],
            Order,
            correlationId);
    }
}

/// <summary>
/// Ordered version of RateLimit filter
/// </summary>
public class OrderedRateLimitAttribute : OrderedFilterAttribute, IActionFilter
{
    private readonly int _maxRequests;
    private readonly int _timeWindowMinutes;

    public OrderedRateLimitAttribute(int maxRequests = 60, int timeWindowMinutes = 1, string? configKey = null)
        : base("RateLimit", configKey)
    {
        _maxRequests = maxRequests;
        _timeWindowMinutes = timeWindowMinutes;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        GetConfiguredOrder(context, 200); // Default order for rate limiting

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OrderedRateLimitAttribute>>();

        logger.LogDebug("🚦 ORDERED RATE LIMIT CHECK: {Controller}.{Action} | Order: {Order} | Limit: {MaxRequests}/{TimeWindow}min",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"],
            Order,
            _maxRequests,
            _timeWindowMinutes);

        // Apply rate limiting logic here (simplified for demo)
        // In real implementation, use the same logic as RateLimitAttribute
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Rate limit cleanup if needed
    }
}

/// <summary>
/// Ordered version of Cache filter
/// </summary>
public class OrderedCacheResponseAttribute : OrderedFilterAttribute, IActionFilter
{
    private readonly int _durationSeconds;

    public OrderedCacheResponseAttribute(int durationSeconds = 300, string? configKey = null)
        : base("CacheResponse", configKey)
    {
        _durationSeconds = durationSeconds;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        GetConfiguredOrder(context, 500); // Default order for caching

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OrderedCacheResponseAttribute>>();

        logger.LogDebug("💾 ORDERED CACHE CHECK: {Controller}.{Action} | Order: {Order} | Duration: {Duration}s",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"],
            Order,
            _durationSeconds);

        // Cache checking logic here
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<OrderedCacheResponseAttribute>>();

        logger.LogDebug("💾 ORDERED CACHE STORE: {Controller}.{Action} | Order: {Order}",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"],
            Order);

        // Cache storing logic here
    }
}