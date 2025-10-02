using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogMVCApp.Filters;

/// <summary>
/// Exception filter for handling specific exceptions at controller/action level
/// </summary>
public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly Type[]? _exceptionTypes;
    private readonly bool _logException;
    private readonly string? _customMessage;

    public CustomExceptionFilterAttribute(Type[]? exceptionTypes = null, bool logException = true, string? customMessage = null)
    {
        _exceptionTypes = exceptionTypes;
        _logException = logException;
        _customMessage = customMessage;
    }

    public override void OnException(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CustomExceptionFilterAttribute>>();
        var exception = context.Exception;

        // Check if we should handle this exception type
        if (_exceptionTypes != null && !_exceptionTypes.Contains(exception.GetType()))
        {
            return; // Let other filters or middleware handle it
        }

        var correlationId = context.HttpContext.TraceIdentifier;
        var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
        var action = context.ActionDescriptor.DisplayName;

        // Log the exception if requested
        if (_logException)
        {
            var customMsg = !string.IsNullOrEmpty(_customMessage) ? $" | Custom: {_customMessage}" : "";
            logger.LogError(exception, "🔥 EXCEPTION FILTER caught exception | User: {User} | Action: {Action} | Exception: {ExceptionType} | Message: {Message} | CorrelationId: {CorrelationId}{CustomMessage}",
                user,
                action,
                exception.GetType().Name,
                exception.Message,
                correlationId,
                customMsg);
        }

        // Handle specific exception types
        HandleSpecificException(context, exception, logger);

        // Mark exception as handled
        context.ExceptionHandled = true;
    }

    private static void HandleSpecificException(ExceptionContext context, Exception exception, ILogger logger)
    {
        var correlationId = context.HttpContext.TraceIdentifier;
        var isApiRequest = IsApiRequest(context.HttpContext);

        switch (exception)
        {
            case ArgumentNullException argNullEx:
                HandleArgumentNullException(context, argNullEx, correlationId, isApiRequest, logger);
                break;

            case ArgumentException argEx:
                HandleArgumentException(context, argEx, correlationId, isApiRequest, logger);
                break;

            case InvalidOperationException invOpEx:
                HandleInvalidOperationException(context, invOpEx, correlationId, isApiRequest, logger);
                break;

            case NotImplementedException notImplEx:
                HandleNotImplementedException(context, notImplEx, correlationId, isApiRequest, logger);
                break;

            case TimeoutException timeoutEx:
                HandleTimeoutException(context, timeoutEx, correlationId, isApiRequest, logger);
                break;

            default:
                HandleGenericException(context, exception, correlationId, isApiRequest, logger);
                break;
        }
    }

    private static void HandleArgumentNullException(ExceptionContext context, ArgumentNullException ex, string correlationId, bool isApiRequest, ILogger logger)
    {
        logger.LogWarning("⚠️ NULL ARGUMENT: Parameter '{Parameter}' is null | Action: {Action}",
            ex.ParamName,
            context.ActionDescriptor.DisplayName);

        if (isApiRequest)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
            {
                Success = false,
                Message = $"Required parameter '{ex.ParamName}' cannot be null",
                CorrelationId = correlationId,
                StatusCode = 400
            })
            { StatusCode = 400 };
        }
        else
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ViewResult
            {
                ViewName = "Error",
                StatusCode = 400,
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["ErrorMessage"] = $"Required parameter '{ex.ParamName}' is missing",
                    ["CorrelationId"] = correlationId
                }
            };
        }
    }

    private static void HandleArgumentException(ExceptionContext context, ArgumentException ex, string correlationId, bool isApiRequest, ILogger logger)
    {
        logger.LogWarning("⚠️ INVALID ARGUMENT: {Message} | Parameter: {Parameter}", ex.Message, ex.ParamName);

        if (isApiRequest)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
            {
                Success = false,
                Message = $"Invalid parameter: {ex.Message}",
                Parameter = ex.ParamName,
                CorrelationId = correlationId,
                StatusCode = 400
            })
            { StatusCode = 400 };
        }
        else
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ViewResult
            {
                ViewName = "Error",
                StatusCode = 400,
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["ErrorMessage"] = $"Invalid input: {ex.Message}",
                    ["CorrelationId"] = correlationId
                }
            };
        }
    }

    private static void HandleInvalidOperationException(ExceptionContext context, InvalidOperationException ex, string correlationId, bool isApiRequest, ILogger logger)
    {
        logger.LogWarning("⚠️ INVALID OPERATION: {Message}", ex.Message);

        if (isApiRequest)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
            {
                Success = false,
                Message = "Operation not valid in current state",
                Details = ex.Message,
                CorrelationId = correlationId,
                StatusCode = 409
            })
            { StatusCode = 409 };
        }
        else
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ViewResult
            {
                ViewName = "Error",
                StatusCode = 409,
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["ErrorMessage"] = "The requested operation cannot be completed at this time",
                    ["CorrelationId"] = correlationId
                }
            };
        }
    }

    private static void HandleNotImplementedException(ExceptionContext context, NotImplementedException ex, string correlationId, bool isApiRequest, ILogger logger)
    {
        logger.LogError("🚧 NOT IMPLEMENTED: {Message} | Action: {Action}", ex.Message, context.ActionDescriptor.DisplayName);

        if (isApiRequest)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
            {
                Success = false,
                Message = "Feature not implemented",
                CorrelationId = correlationId,
                StatusCode = 501
            })
            { StatusCode = 501 };
        }
        else
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ViewResult
            {
                ViewName = "Error",
                StatusCode = 501,
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["ErrorMessage"] = "This feature is not yet available",
                    ["CorrelationId"] = correlationId
                }
            };
        }
    }

    private static void HandleTimeoutException(ExceptionContext context, TimeoutException ex, string correlationId, bool isApiRequest, ILogger logger)
    {
        logger.LogWarning("⏰ TIMEOUT: Operation timed out | {Message}", ex.Message);

        if (isApiRequest)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
            {
                Success = false,
                Message = "Operation timed out. Please try again.",
                CorrelationId = correlationId,
                StatusCode = 408
            })
            { StatusCode = 408 };
        }
        else
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ViewResult
            {
                ViewName = "Error",
                StatusCode = 408,
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["ErrorMessage"] = "The operation took too long to complete. Please try again.",
                    ["CorrelationId"] = correlationId
                }
            };
        }
    }

    private static void HandleGenericException(ExceptionContext context, Exception ex, string correlationId, bool isApiRequest, ILogger logger)
    {
        logger.LogError(ex, "🔥 GENERIC EXCEPTION handled by filter | Type: {ExceptionType}", ex.GetType().Name);

        if (isApiRequest)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new
            {
                Success = false,
                Message = "An unexpected error occurred",
                CorrelationId = correlationId,
                StatusCode = 500
            })
            { StatusCode = 500 };
        }
        else
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ViewResult
            {
                ViewName = "Error",
                StatusCode = 500,
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    ["ErrorMessage"] = "An unexpected error occurred. Please try again later.",
                    ["CorrelationId"] = correlationId
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
}