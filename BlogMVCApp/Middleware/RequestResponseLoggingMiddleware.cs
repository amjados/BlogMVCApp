using System.Diagnostics;
using System.Text;

namespace BlogMVCApp.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var correlationId = context.TraceIdentifier;

            // Log request
            await LogRequestAsync(context, correlationId);

            // Capture response
            var originalResponseStream = context.Response.Body;
            using var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            try
            {
                await _next(context);
                stopwatch.Stop();

                // Log response
                await LogResponseAsync(context, correlationId, stopwatch.ElapsedMilliseconds);

                // Copy response back to original stream
                responseStream.Position = 0;
                await responseStream.CopyToAsync(originalResponseStream);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "❌ Request failed. CorrelationId: {CorrelationId}, Duration: {Duration}ms, Path: {Path}",
                    correlationId, stopwatch.ElapsedMilliseconds, context.Request.Path);

                // Still copy any response that was written
                responseStream.Position = 0;
                await responseStream.CopyToAsync(originalResponseStream);
                throw;
            }
            finally
            {
                context.Response.Body = originalResponseStream;
            }
        }

        private async Task LogRequestAsync(HttpContext context, string correlationId)
        {
            var request = context.Request;

            // Don't log sensitive endpoints or static files
            if (IsSensitiveEndpoint(request.Path) || IsStaticFile(request.Path))
                return;

            var requestBody = "";
            if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                // Truncate long request bodies
                if (requestBody.Length > 1000)
                {
                    requestBody = requestBody[..1000] + "... [truncated]";
                }
            }

            _logger.LogInformation(
                "📨 HTTP Request - {Method} {Path} | User: {User} | IP: {IP} | UserAgent: {UserAgent} | CorrelationId: {CorrelationId} | ContentType: {ContentType} | Body: {Body}",
                request.Method,
                request.Path + request.QueryString,
                context.User?.Identity?.Name ?? "Anonymous",
                GetClientIP(context),
                request.Headers.UserAgent.ToString().Truncate(100),
                correlationId,
                request.ContentType ?? "none",
                string.IsNullOrEmpty(requestBody) ? "[empty]" : requestBody
            );
        }

        private async Task LogResponseAsync(HttpContext context, string correlationId, long duration)
        {
            var response = context.Response;

            // Don't log sensitive endpoints or static files
            if (IsSensitiveEndpoint(context.Request.Path) || IsStaticFile(context.Request.Path))
                return;

            var responseBody = "";
            if (response.ContentType?.Contains("application/json") == true && response.Body.CanRead)
            {
                context.Response.Body.Position = 0;
                using var reader = new StreamReader(context.Response.Body, leaveOpen: true);
                responseBody = await reader.ReadToEndAsync();
                context.Response.Body.Position = 0;

                // Truncate long response bodies
                if (responseBody.Length > 1000)
                {
                    responseBody = responseBody[..1000] + "... [truncated]";
                }
            }

            var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            var emoji = response.StatusCode switch
            {
                >= 200 and < 300 => "✅",
                >= 300 and < 400 => "↩️",
                >= 400 and < 500 => "⚠️",
                >= 500 => "❌",
                _ => "ℹ️"
            };

            _logger.Log(logLevel,
                "{Emoji} HTTP Response - {StatusCode} {StatusText} | Duration: {Duration}ms | Size: {Size} bytes | CorrelationId: {CorrelationId} | ContentType: {ContentType} | Body: {Body}",
                emoji,
                response.StatusCode,
                GetStatusCodeText(response.StatusCode),
                duration,
                response.ContentLength ?? 0,
                correlationId,
                response.ContentType ?? "none",
                string.IsNullOrEmpty(responseBody) ? "[empty]" : responseBody
            );

            // Log slow requests separately
            if (duration > 2000) // 2 seconds
            {
                _logger.LogWarning(
                    "🐌 VERY SLOW REQUEST: {Method} {Path} took {Duration}ms | User: {User}",
                    context.Request.Method,
                    context.Request.Path,
                    duration,
                    context.User?.Identity?.Name ?? "Anonymous"
                );
            }
        }

        private static bool IsSensitiveEndpoint(string path)
        {
            var sensitiveEndpoints = new[]
            {
                "/Identity/Account/Login",
                "/Identity/Account/Register",
                "/api/auth",
                "/Account/Login",
                "/Account/Register",
                "/login",
                "/register"
            };
            return sensitiveEndpoints.Any(endpoint => path.StartsWith(endpoint, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsStaticFile(string path)
        {
            var staticExtensions = new[] { ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".ico", ".svg", ".woff", ".woff2", ".ttf", ".eot" };
            return staticExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetClientIP(HttpContext context)
        {
            return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim() ??
                   context.Request.Headers["X-Real-IP"].FirstOrDefault() ??
                   context.Connection.RemoteIpAddress?.ToString() ??
                   "Unknown";
        }

        private static string GetStatusCodeText(int statusCode)
        {
            return statusCode switch
            {
                200 => "OK",
                201 => "Created",
                204 => "No Content",
                301 => "Moved Permanently",
                302 => "Found",
                304 => "Not Modified",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                500 => "Internal Server Error",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                _ => "Unknown"
            };
        }
    }

    // Extension method for string truncation
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;

            return value[..maxLength] + "...";
        }
    }
}