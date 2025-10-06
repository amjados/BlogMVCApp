using System.Diagnostics;

namespace BlogMVCApp.Middleware
{
    public class PerformanceMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMonitoringMiddleware> _logger;
        private readonly long _slowRequestThreshold;
        private readonly long _verySlowRequestThreshold;

        public PerformanceMonitoringMiddleware(
            RequestDelegate next,
            ILogger<PerformanceMonitoringMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _slowRequestThreshold = configuration.GetValue<long>("Performance:SlowRequestThresholdMs", 1000);
            _verySlowRequestThreshold = configuration.GetValue<long>("Performance:VerySlowRequestThresholdMs", 5000);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip performance monitoring for static files
            if (IsStaticFile(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                var memoryAfter = GC.GetTotalMemory(false);
                var memoryUsed = memoryAfter - memoryBefore;

                // Only try to add headers if response hasn't started
                if (!context.Response.HasStarted)
                {
                    context.Response.Headers.TryAdd("X-Response-Time", $"{duration}ms");
                    context.Response.Headers.TryAdd("X-Memory-Used", $"{memoryUsed} bytes");
                }

                // Log performance metrics
                LogPerformanceMetrics(context, duration, memoryUsed);
            }
        }

        private void LogPerformanceMetrics(HttpContext context, long duration, long memoryUsed)
        {
            var request = context.Request;
            var response = context.Response;

            var logLevel = LogLevel.Information;
            if (duration >= _verySlowRequestThreshold)
            {
                logLevel = LogLevel.Error;    // 5+ seconds - critical
            }
            else if (duration >= 2000)
            {
                logLevel = LogLevel.Warning;  // 2-5 seconds - warning
            }
            else if (duration >= _slowRequestThreshold)
            {
                logLevel = LogLevel.Information; // 1-2 seconds - info
            }
            else
            {
                logLevel = LogLevel.Debug;           // < 1 second - debug
            }

            var emoji = "🚀"; // Default fast
            if (duration >= _verySlowRequestThreshold)
            {
                emoji = "🔥"; // Very slow
            }
            else if (duration >= _slowRequestThreshold)
            {
                emoji = "🐌"; // Slow
            }
            else if (duration >= 500)
            {
                emoji = "⚡"; // Moderate
            }

            _logger.Log(logLevel,
                "{Emoji} PERFORMANCE: {Method} {Path} | Duration: {Duration}ms | Memory: {Memory} bytes | Status: {StatusCode} | User: {User} | IP: {IP}",
                emoji,
                request.Method,
                request.Path,
                duration,
                memoryUsed,
                response.StatusCode,
                context.User?.Identity?.Name ?? "Anonymous",
                GetClientIP(context)
            );

            // Log additional details for slow requests
            if (duration >= _slowRequestThreshold)
            {
                _logger.LogWarning(
                    "🐌 SLOW REQUEST DETAILS: {Method} {Path} | Duration: {Duration}ms (threshold: {Threshold}ms) | Memory Used: {Memory} bytes | Query String: {QueryString} | User: {User} | UserAgent: {UserAgent}",
                    request.Method,
                    request.Path,
                    duration,
                    _slowRequestThreshold,
                    memoryUsed,
                    request.QueryString,
                    context.User?.Identity?.Name ?? "Anonymous",
                    request.Headers.UserAgent.ToString().Truncate(100)
                );
            }

            // Log critical performance issues
            if (duration >= _verySlowRequestThreshold)
            {
                _logger.LogError(
                    "🔥 CRITICAL PERFORMANCE ISSUE: {Method} {Path} took {Duration}ms! This requires immediate attention. Memory: {Memory} bytes | User: {User}",
                    request.Method,
                    request.Path,
                    duration,
                    memoryUsed,
                    context.User?.Identity?.Name ?? "Anonymous"
                );
            }

            // Log high memory usage
            if (memoryUsed > 10 * 1024 * 1024) // 10MB
            {
                _logger.LogWarning(
                    "🧠 HIGH MEMORY USAGE: {Method} {Path} used {Memory} bytes ({MemoryMB} MB) | Duration: {Duration}ms",
                    request.Method,
                    request.Path,
                    memoryUsed,
                    memoryUsed / 1024 / 1024,
                    duration
                );
            }

            // Collect additional metrics for monitoring
            CollectAdditionalMetrics(context, duration, memoryUsed);
        }

        private void CollectAdditionalMetrics(HttpContext context, long duration, long memoryUsed)
        {
            // You can extend this to send metrics to monitoring systems
            // like Application Insights, Prometheus, etc.

            var metrics = new Dictionary<string, object>
            {
                ["RequestPath"] = context.Request.Path.ToString(),
                ["RequestMethod"] = context.Request.Method,
                ["ResponseStatusCode"] = context.Response.StatusCode,
                ["Duration"] = duration,
                ["MemoryUsed"] = memoryUsed,
                ["Timestamp"] = DateTimeOffset.UtcNow,
                ["UserId"] = context.User?.Identity?.Name ?? "Anonymous",
                ["UserAgent"] = context.Request.Headers.UserAgent.ToString(),
                ["IpAddress"] = GetClientIP(context) ?? "Unknown"
            };

            // Log structured performance data for analytics
            _logger.LogInformation(
                "📊 PERFORMANCE_METRICS: Path={Path} Method={Method} StatusCode={StatusCode} Duration={Duration} Memory={Memory} User={User}",
                context.Request.Path.ToString(),
                context.Request.Method,
                context.Response.StatusCode,
                duration,
                memoryUsed,
                context.User?.Identity?.Name ?? "Anonymous"
            );
        }

        private static bool IsStaticFile(string path)
        {
            var staticExtensions = new[]
            {
                ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".ico", ".svg",
                ".woff", ".woff2", ".ttf", ".eot", ".map", ".pdf", ".zip"
            };
            return staticExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetClientIP(HttpContext context)
        {
            return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim() ??
                   context.Request.Headers["X-Real-IP"].FirstOrDefault() ??
                   context.Connection.RemoteIpAddress?.ToString() ??
                   "Unknown";
        }
    }
}