namespace BlogMVCApp.Configuration;

/// <summary>
/// Configuration settings for Action Filters
/// </summary>
public class FilterConfiguration
{
    public const string SectionName = "Filters";

    public RateLimitingConfig RateLimiting { get; set; } = new();
    public CachingConfig Caching { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();
    public ValidationConfig Validation { get; set; } = new();
    public AuthorizationConfig Authorization { get; set; } = new();
}

public class RateLimitingConfig
{
    /// <summary>
    /// Default rate limit settings for actions
    /// </summary>
    public DefaultRateLimit Default { get; set; } = new();

    /// <summary>
    /// Per-action rate limit overrides
    /// </summary>
    public Dictionary<string, ActionRateLimit> Actions { get; set; } = new();
}

public class DefaultRateLimit
{
    public int MaxRequests { get; set; } = 60;
    public int TimeWindowMinutes { get; set; } = 1;
    public bool PerUser { get; set; } = true;
}

public class ActionRateLimit
{
    public int MaxRequests { get; set; }
    public int TimeWindowMinutes { get; set; }
    public bool PerUser { get; set; }
}

public class CachingConfig
{
    /// <summary>
    /// Default cache settings for actions
    /// </summary>
    public DefaultCache Default { get; set; } = new();

    /// <summary>
    /// Per-action cache overrides
    /// </summary>
    public Dictionary<string, ActionCache> Actions { get; set; } = new();
}

public class DefaultCache
{
    public int DurationSeconds { get; set; } = 300; // 5 minutes
    public bool VaryByUser { get; set; } = false;
    public bool VaryByQueryString { get; set; } = true;
}

public class ActionCache
{
    public int DurationSeconds { get; set; }
    public bool VaryByUser { get; set; }
    public bool VaryByQueryString { get; set; }
}

public class LoggingConfig
{
    public bool EnableActionLogging { get; set; } = true;
    public bool LogParameters { get; set; } = true;
    public bool LogPerformance { get; set; } = true;
    public string[] ExcludeActions { get; set; } = Array.Empty<string>();
}

public class ValidationConfig
{
    public bool EnableModelValidation { get; set; } = true;
    public bool ReturnDetailedErrors { get; set; } = true;
    public bool LogValidationFailures { get; set; } = true;
}

public class AuthorizationConfig
{
    public bool EnableCustomAuthorization { get; set; } = true;
    public bool LogAuthorizationEvents { get; set; } = true;
    public Dictionary<string, string[]> RoleRequirements { get; set; } = new();
}