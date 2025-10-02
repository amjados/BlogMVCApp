using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogMVCApp.Filters;

/// <summary>
/// Action filter that validates model state and returns appropriate responses for invalid models
/// </summary>
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ValidateModelAttribute>>();
            var correlationId = context.HttpContext.TraceIdentifier;

            // Log model validation failure
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => new { Field = x.Key, Message = e.ErrorMessage }))
                .ToList();

            logger.LogWarning("📝 MODEL VALIDATION failed | CorrelationId: {CorrelationId} | Action: {Action} | Errors: {@Errors}",
                correlationId,
                context.ActionDescriptor.DisplayName,
                errors);

            // Check if this is an API request (Accept header contains application/json)
            var acceptHeader = context.HttpContext.Request.Headers["Accept"].ToString();
            var isApiRequest = acceptHeader.Contains("application/json", StringComparison.OrdinalIgnoreCase) ||
                              context.HttpContext.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);

            if (isApiRequest)
            {
                // Return JSON response for API requests
                var errorResponse = new
                {
                    Success = false,
                    Message = "Validation failed",
                    CorrelationId = correlationId,
                    Errors = errors.ToDictionary(e => e.Field, e => e.Message)
                };

                context.Result = new BadRequestObjectResult(errorResponse);
            }
            else
            {
                // For web requests, let the controller handle it (return to view with validation errors)
                // The ModelState errors will be automatically displayed in the view
                logger.LogInformation("🌐 WEB REQUEST with validation errors - returning to view for user correction");
            }
        }

        base.OnActionExecuting(context);
    }
}