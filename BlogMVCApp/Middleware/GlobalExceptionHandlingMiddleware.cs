using System.Net;
using System.Text.Json;
using System.Security.Claims;

namespace BlogMVCApp.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = Guid.NewGuid().ToString();

            // Log the exception with context information
            _logger.LogError(exception,
                "❌ Global exception caught. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}, User: {User}, IP: {IP}",
                correlationId,
                context.Request.Path,
                context.Request.Method,
                context.User?.Identity?.Name ?? "Anonymous",
                GetClientIP(context));

            // Determine response based on exception type
            var response = exception switch
            {
                NotFoundException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Resource not found",
                    CorrelationId = correlationId
                },
                UnauthorizedAccessException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Access denied",
                    CorrelationId = correlationId
                },
                ValidationException validationEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Validation failed",
                    Details = validationEx.Errors,
                    CorrelationId = correlationId
                },
                ArgumentException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid request data",
                    CorrelationId = correlationId
                },
                InvalidOperationException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid operation",
                    CorrelationId = correlationId
                },
                _ => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An error occurred while processing your request",
                    CorrelationId = correlationId
                }
            };

            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";

            // Add correlation ID to response headers
            context.Response.Headers["X-Correlation-Id"] = correlationId;
            context.Response.Headers["X-Error-Timestamp"] = DateTimeOffset.UtcNow.ToString("O");

            // Return JSON response for API calls, redirect for web requests
            if (IsApiRequest(context))
            {
                var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                // Store error details in TempData for error page
                context.Items["ErrorCorrelationId"] = correlationId;
                context.Items["ErrorMessage"] = response.Message;
                context.Items["ErrorStatusCode"] = response.StatusCode;

                // Redirect to error page for web requests
                context.Response.Redirect($"/Home/Error?correlationId={correlationId}&statusCode={response.StatusCode}");
            }
        }

        private static bool IsApiRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/api") ||
                   context.Request.Headers.Accept.Any(h => h?.Contains("application/json") == true) ||
                   context.Request.ContentType?.Contains("application/json") == true;
        }

        private static string GetClientIP(HttpContext context)
        {
            return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim() ??
                   context.Request.Headers["X-Real-IP"].FirstOrDefault() ??
                   context.Connection.RemoteIpAddress?.ToString() ??
                   "Unknown";
        }
    }

    // Custom exceptions
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors) : base("Validation failed")
        {
            Errors = errors ?? new List<string>();
        }

        public ValidationException(string error) : base("Validation failed")
        {
            Errors = new List<string> { error };
        }
    }

    // Error response model
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public IEnumerable<string>? Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}