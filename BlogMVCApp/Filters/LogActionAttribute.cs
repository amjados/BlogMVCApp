using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace BlogMVCApp.Filters;

/// <summary>
/// Action filter that logs detailed information about action execution
/// </summary>
public class LogActionAttribute : ActionFilterAttribute
{
    private readonly string? _customMessage;

    public LogActionAttribute(string? customMessage = null)
    {
        _customMessage = customMessage;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LogActionAttribute>>();
        var correlationId = context.HttpContext.TraceIdentifier;
        var user = context.HttpContext.User;
        var userName = user.Identity?.IsAuthenticated == true ? user.Identity.Name : "Anonymous";
        var userRoles = user.Identity?.IsAuthenticated == true ? string.Join(", ", user.Claims
            .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)) : "None";

        // Store start time for performance measurement
        context.HttpContext.Items["ActionStartTime"] = Stopwatch.GetTimestamp();

        // Log action parameters (excluding sensitive data)
        var parameters = context.ActionArguments
            .Where(p => !IsSensitiveParameter(p.Key))
            .ToDictionary(p => p.Key, p => p.Value?.ToString() ?? "null");

        var message = !string.IsNullOrEmpty(_customMessage) ? $" | Custom: {_customMessage}" : "";

        logger.LogInformation("🎬 ACTION STARTING: {Controller}.{Action} | User: {User} | Roles: [{Roles}] | CorrelationId: {CorrelationId} | Parameters: {@Parameters}{Message}",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"],
            userName,
            userRoles,
            correlationId,
            parameters,
            message);

        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LogActionAttribute>>();
        var correlationId = context.HttpContext.TraceIdentifier;

        // Calculate execution time
        var startTime = (long?)context.HttpContext.Items["ActionStartTime"];
        var duration = startTime.HasValue
            ? (double)(Stopwatch.GetTimestamp() - startTime.Value) / Stopwatch.Frequency * 1000
            : 0;

        var resultType = context.Result?.GetType().Name ?? "Unknown";
        var statusCode = context.HttpContext.Response.StatusCode;

        if (context.Exception != null)
        {
            logger.LogError("💥 ACTION FAILED: {Controller}.{Action} | Duration: {Duration:F2}ms | Exception: {Exception} | CorrelationId: {CorrelationId}",
                context.RouteData.Values["controller"],
                context.RouteData.Values["action"],
                duration,
                context.Exception.Message,
                correlationId);
        }
        else
        {
            var logLevel = duration > 1000 ? LogLevel.Warning : LogLevel.Information;
            var emoji = duration > 1000 ? "🐌" : "✅";

            logger.Log(logLevel, "{Emoji} ACTION COMPLETED: {Controller}.{Action} | Duration: {Duration:F2}ms | Result: {ResultType} | Status: {StatusCode} | CorrelationId: {CorrelationId}",
                emoji,
                context.RouteData.Values["controller"],
                context.RouteData.Values["action"],
                duration,
                resultType,
                statusCode,
                correlationId);
        }

        base.OnActionExecuted(context);
    }

    private static bool IsSensitiveParameter(string parameterName)
    {
        var sensitiveParams = new[] { "password", "token", "secret", "key", "auth", "credential" };
        return sensitiveParams.Any(sp => parameterName.Contains(sp, StringComparison.OrdinalIgnoreCase));
    }
}