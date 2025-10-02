using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogMVCApp.Filters;

/// <summary>
/// Authorization filter for custom role-based and complex authorization logic
/// </summary>
public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[]? _roles;
    private readonly string[]? _policies;
    private readonly bool _requireAllRoles;
    private readonly bool _allowAnonymous;

    public CustomAuthorizeAttribute(string? roles = null, string? policies = null, bool requireAllRoles = false, bool allowAnonymous = false)
    {
        _roles = roles?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
        _policies = policies?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();
        _requireAllRoles = requireAllRoles;
        _allowAnonymous = allowAnonymous;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CustomAuthorizeAttribute>>();
        var correlationId = context.HttpContext.TraceIdentifier;
        var user = context.HttpContext.User;

        // Allow anonymous access if specified
        if (_allowAnonymous)
        {
            logger.LogDebug("🔓 ANONYMOUS ACCESS allowed for {Action}", context.ActionDescriptor.DisplayName);
            return;
        }

        // Check if user is authenticated
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            logger.LogWarning("🔒 AUTHENTICATION required | Action: {Action} | IP: {IP} | CorrelationId: {CorrelationId}",
                context.ActionDescriptor.DisplayName,
                context.HttpContext.Connection.RemoteIpAddress,
                correlationId);

            HandleUnauthorized(context, "Authentication required");
            return;
        }

        var userName = user.Identity?.Name ?? "Unknown";
        var userRoles = user.Claims
            .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        // Check roles if specified
        if (_roles?.Length > 0)
        {
            var hasRequiredRoles = _requireAllRoles
                ? _roles.All(role => userRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                : _roles.Any(role => userRoles.Contains(role, StringComparer.OrdinalIgnoreCase));

            if (!hasRequiredRoles)
            {
                logger.LogWarning("🚫 AUTHORIZATION failed - Insufficient roles | User: {User} | UserRoles: [{UserRoles}] | RequiredRoles: [{RequiredRoles}] | RequireAll: {RequireAll} | Action: {Action} | CorrelationId: {CorrelationId}",
                    userName,
                    string.Join(", ", userRoles),
                    string.Join(", ", _roles),
                    _requireAllRoles,
                    context.ActionDescriptor.DisplayName,
                    correlationId);

                HandleForbidden(context, "Insufficient role permissions");
                return;
            }
        }

        // Check custom policies if specified
        if (_policies?.Length > 0)
        {
            var authorizationService = context.HttpContext.RequestServices.GetService<Microsoft.AspNetCore.Authorization.IAuthorizationService>();

            if (authorizationService != null)
            {
                foreach (var policy in _policies)
                {
                    var authResult = await authorizationService.AuthorizeAsync(user, null, policy);
                    if (!authResult.Succeeded)
                    {
                        logger.LogWarning("🚫 POLICY AUTHORIZATION failed | User: {User} | Policy: {Policy} | Reasons: {Reasons} | Action: {Action} | CorrelationId: {CorrelationId}",
                            userName,
                            policy,
                            string.Join(", ", authResult.Failure?.FailureReasons.Select(r => r.Message) ?? new[] { "Unknown" }),
                            context.ActionDescriptor.DisplayName,
                            correlationId);

                        HandleForbidden(context, $"Policy '{policy}' not satisfied");
                        return;
                    }
                }
            }
        }

        // Additional custom authorization logic can be added here
        if (!PerformCustomChecks(context, user))
        {
            logger.LogWarning("🚫 CUSTOM AUTHORIZATION failed | User: {User} | Action: {Action} | CorrelationId: {CorrelationId}",
                userName,
                context.ActionDescriptor.DisplayName,
                correlationId);

            HandleForbidden(context, "Custom authorization check failed");
            return;
        }

        logger.LogInformation("✅ AUTHORIZATION successful | User: {User} | Roles: [{Roles}] | Action: {Action} | CorrelationId: {CorrelationId}",
            userName,
            string.Join(", ", userRoles),
            context.ActionDescriptor.DisplayName,
            correlationId);
    }

    private static void HandleUnauthorized(AuthorizationFilterContext context, string message)
    {
        var isApiRequest = IsApiRequest(context.HttpContext);

        if (isApiRequest)
        {
            var errorResponse = new
            {
                Success = false,
                Message = message,
                CorrelationId = context.HttpContext.TraceIdentifier,
                StatusCode = 401
            };

            context.Result = new JsonResult(errorResponse) { StatusCode = 401 };
        }
        else
        {
            // Redirect to login page for web requests
            var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            var loginUrl = $"/Home/Login?returnUrl={Uri.EscapeDataString(returnUrl)}";
            context.Result = new RedirectResult(loginUrl);
        }
    }

    private static void HandleForbidden(AuthorizationFilterContext context, string message)
    {
        var isApiRequest = IsApiRequest(context.HttpContext);

        if (isApiRequest)
        {
            var errorResponse = new
            {
                Success = false,
                Message = message,
                CorrelationId = context.HttpContext.TraceIdentifier,
                StatusCode = 403
            };

            context.Result = new JsonResult(errorResponse) { StatusCode = 403 };
        }
        else
        {
            // Show access denied page for web requests
            context.Result = new ViewResult
            {
                ViewName = "AccessDenied",
                StatusCode = 403,
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["Message"] = message
                }
            };
        }
    }

    private static bool IsApiRequest(HttpContext httpContext)
    {
        var acceptHeader = httpContext.Request.Headers["Accept"].ToString();
        return acceptHeader.Contains("application/json", StringComparison.OrdinalIgnoreCase) ||
               httpContext.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
    }

    private static bool PerformCustomChecks(AuthorizationFilterContext context, System.Security.Claims.ClaimsPrincipal user)
    {
        // Add your custom authorization logic here
        // Examples:

        // 1. Time-based access control
        var currentHour = DateTime.Now.Hour;
        if (currentHour < 6 || currentHour > 22) // Only allow access between 6 AM and 10 PM
        {
            // return false; // Uncomment to enable time-based restriction
        }

        // 2. IP-based restrictions
        var clientIP = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        var restrictedIPs = new[] { "192.168.1.100", "10.0.0.50" }; // Example restricted IPs
        if (!string.IsNullOrEmpty(clientIP) && restrictedIPs.Contains(clientIP))
        {
            // return false; // Uncomment to enable IP restrictions
        }

        // 3. User-specific restrictions
        var userName = user.Identity?.Name;
        if (!string.IsNullOrEmpty(userName))
        {
            // Check if user is suspended, locked, etc.
            // You could query database here for user status

            // Example: Check for suspended users
            var suspendedUsers = new[] { "suspended@example.com" };
            if (suspendedUsers.Contains(userName, StringComparer.OrdinalIgnoreCase))
            {
                // return false; // Uncomment to enable suspended user check
            }
        }

        // 4. Request frequency check (basic rate limiting at user level)
        // This could integrate with a more sophisticated rate limiting system

        return true; // All custom checks passed
    }
}