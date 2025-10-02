using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BlogMVCApp.Configuration;
using BlogMVCApp.Filters;

namespace BlogMVCApp.Providers;

/// <summary>
/// Filter provider that applies filters based on configuration
/// </summary>
public class ConfigurationBasedFilterProvider : IFilterProvider
{
    private readonly FilterManager _filterManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConfigurationBasedFilterProvider> _logger;

    public ConfigurationBasedFilterProvider(
        FilterManager filterManager,
        IServiceProvider serviceProvider,
        ILogger<ConfigurationBasedFilterProvider> logger)
    {
        _filterManager = filterManager;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public int Order => -1000; // Execute early to set up filters

    public void OnProvidersExecuted(FilterProviderContext context)
    {
        // Nothing to do after execution
    }

    public void OnProvidersExecuting(FilterProviderContext context)
    {
        if (context.ActionContext.ActionDescriptor is not Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor actionDescriptor)
            return;

        var controllerName = actionDescriptor.ControllerName;
        var actionName = actionDescriptor.ActionName;

        // Get filters for this action from configuration
        var configuredFilters = _filterManager.GetFiltersForAction(controllerName, actionName);

        foreach (var filterDef in configuredFilters)
        {
            try
            {
                var filter = CreateFilter(filterDef);
                if (filter != null)
                {
                    var filterDescriptor = new FilterDescriptor(filter, FilterScope.Action)
                    {
                        Order = filterDef.Order
                    };

                    var filterItem = new FilterItem(filterDescriptor, filter);

                    context.Results.Add(filterItem);

                    _logger.LogDebug("📌 APPLIED CONFIG FILTER: {FilterType} (Order: {Order}) to {Controller}.{Action}",
                        filterDef.FilterType, filterDef.Order, controllerName, actionName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to create filter {FilterType} for {Controller}.{Action}",
                    filterDef.FilterType, controllerName, actionName);
            }
        }
    }

    private IFilterMetadata? CreateFilter(FilterDefinition filterDef)
    {
        return filterDef.FilterType.ToLower() switch
        {
            "logaction" => new LogActionAttribute(
                filterDef.Parameters.GetValueOrDefault("message", "Config-based logging")?.ToString() ?? "Config-based logging"),

            "configurableratelimit" => new ConfigurableRateLimitAttribute(
                filterDef.Parameters.GetValueOrDefault("actionKey")?.ToString() ?? "default"),

            "configurablecacheresponse" => new ConfigurableCacheResponseAttribute(
                filterDef.Parameters.GetValueOrDefault("actionKey")?.ToString() ?? "default"),

            "validatemodel" => new ValidateModelAttribute(),

            "customauthorize" => new CustomAuthorizeAttribute(
                filterDef.Parameters.GetValueOrDefault("roles")?.ToString() ?? ""),

            "customexceptionfilter" => new CustomExceptionFilterAttribute(
                logException: filterDef.Parameters.GetValueOrDefault("logException", true) is bool log && log,
                customMessage: filterDef.Parameters.GetValueOrDefault("customMessage")?.ToString()),

            // Add more filter types as needed
            _ => null
        };
    }
}

/// <summary>
/// Extension methods for filter configuration
/// </summary>
public static class FilterConfigurationExtensions
{
    /// <summary>
    /// Adds configuration-based filter provider to the MVC options
    /// </summary>
    public static void AddConfigurationBasedFilters(this MvcOptions options, IServiceProvider serviceProvider)
    {
        // Note: Filter providers are registered in DI container and applied automatically
        // This extension method is kept for backward compatibility
    }
}