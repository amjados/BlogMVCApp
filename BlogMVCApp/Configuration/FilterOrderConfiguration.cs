using BlogMVCApp.Filters;
using BlogMVCApp.Configuration;
using Microsoft.Extensions.Options;

namespace BlogMVCApp.Configuration;

/// <summary>
/// Configuration for controlling filter order and global application
/// </summary>
public class FilterOrderConfiguration
{
    public const string SectionName = "FilterOrder";

    /// <summary>
    /// Global filters applied to all actions in specified order
    /// </summary>
    public List<FilterDefinition> GlobalFilters { get; set; } = new();

    /// <summary>
    /// Controller-specific filter overrides
    /// </summary>
    public Dictionary<string, List<FilterDefinition>> ControllerFilters { get; set; } = new();

    /// <summary>
    /// Action-specific filter overrides
    /// </summary>
    public Dictionary<string, List<FilterDefinition>> ActionFilters { get; set; } = new();

    /// <summary>
    /// Default filter order for all filter types
    /// </summary>
    public FilterOrderSettings DefaultOrder { get; set; } = new();
}

public class FilterDefinition
{
    public required string FilterType { get; set; }
    public int Order { get; set; }
    public bool Enabled { get; set; } = true;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public List<string> ExcludeActions { get; set; } = new();
    public List<string> IncludeActions { get; set; } = new();
}

public class FilterOrderSettings
{
    /// <summary>
    /// Order for different filter types (lower numbers execute first)
    /// </summary>
    public int AuthorizationOrder { get; set; } = 100;
    public int RateLimitOrder { get; set; } = 200;
    public int ValidationOrder { get; set; } = 300;
    public int LoggingOrder { get; set; } = 400;
    public int CachingOrder { get; set; } = 500;
    public int ExceptionOrder { get; set; } = 600;
}

/// <summary>
/// Service to manage filter application based on configuration
/// </summary>
public class FilterManager
{
    private readonly FilterOrderConfiguration _config;
    private readonly ILogger<FilterManager> _logger;

    public FilterManager(IOptions<FilterOrderConfiguration> config, ILogger<FilterManager> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    /// <summary>
    /// Gets the filters that should be applied to a specific action
    /// </summary>
    public List<FilterDefinition> GetFiltersForAction(string controllerName, string actionName)
    {
        var filters = new List<FilterDefinition>();

        // 1. Start with global filters
        filters.AddRange(_config.GlobalFilters.Where(f => f.Enabled));

        // 2. Add controller-specific filters
        if (_config.ControllerFilters.ContainsKey(controllerName))
        {
            filters.AddRange(_config.ControllerFilters[controllerName].Where(f => f.Enabled));
        }

        // 3. Add action-specific filters
        var actionKey = $"{controllerName}.{actionName}";
        if (_config.ActionFilters.ContainsKey(actionKey))
        {
            filters.AddRange(_config.ActionFilters[actionKey].Where(f => f.Enabled));
        }

        // 4. Filter out excluded actions
        filters = filters.Where(f =>
            !f.ExcludeActions.Contains(actionName) &&
            !f.ExcludeActions.Contains(actionKey) &&
            (f.IncludeActions.Count == 0 || f.IncludeActions.Contains(actionName) || f.IncludeActions.Contains(actionKey))
        ).ToList();

        // 5. Sort by order
        filters = filters.OrderBy(f => f.Order).ToList();

        _logger.LogDebug("📋 FILTER ORDER for {Controller}.{Action}: {Filters}",
            controllerName, actionName,
            string.Join(" → ", filters.Select(f => f.FilterType)));

        return filters;
    }

    /// <summary>
    /// Gets the default order for a filter type
    /// </summary>
    public int GetDefaultOrder(string filterType)
    {
        return filterType.ToLower() switch
        {
            "authorization" or "customauthorize" => _config.DefaultOrder.AuthorizationOrder,
            "ratelimit" or "configurableratelimit" => _config.DefaultOrder.RateLimitOrder,
            "validation" or "validatemodel" => _config.DefaultOrder.ValidationOrder,
            "logging" or "logaction" => _config.DefaultOrder.LoggingOrder,
            "caching" or "cacheresponse" or "configurablecacheresponse" => _config.DefaultOrder.CachingOrder,
            "exception" or "customexceptionfilter" => _config.DefaultOrder.ExceptionOrder,
            _ => 999
        };
    }
}