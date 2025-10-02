using System.Security.Claims;

namespace BlogMVCApp.Middleware
{
    public class SecurityLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityLoggingMiddleware> _logger;
        private readonly Dictionary<string, int> _ipRequestCounts = new();
        private readonly Dictionary<string, DateTime> _ipLastRequest = new();
        private readonly int _maxRequestsPerMinute;

        public SecurityLoggingMiddleware(RequestDelegate next, ILogger<SecurityLoggingMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _maxRequestsPerMinute = configuration.GetValue<int>("Security:MaxRequestsPerMinute", 100);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = GetClientIP(context);

            // Check for rate limiting
            CheckRateLimit(context, clientIp);

            // Log authentication info before processing
            LogAuthenticationInfo(context);

            // Check for suspicious activity before processing
            LogSuspiciousActivity(context);

            await _next(context);

            // Log security events after processing
            LogSecurityEvents(context, clientIp);
        }

        private void CheckRateLimit(HttpContext context, string clientIp)
        {
            var now = DateTime.UtcNow;

            // Clean old entries (older than 1 minute)
            var keysToRemove = _ipLastRequest
                .Where(kvp => now - kvp.Value > TimeSpan.FromMinutes(1))
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _ipRequestCounts.Remove(key);
                _ipLastRequest.Remove(key);
            }

            // Check current IP
            if (_ipLastRequest.ContainsKey(clientIp))
            {
                var timeSinceLastRequest = now - _ipLastRequest[clientIp];
                if (timeSinceLastRequest < TimeSpan.FromMinutes(1))
                {
                    _ipRequestCounts[clientIp] = _ipRequestCounts.GetValueOrDefault(clientIp, 0) + 1;

                    if (_ipRequestCounts[clientIp] > _maxRequestsPerMinute)
                    {
                        _logger.LogWarning(
                            "🚨 RATE LIMIT EXCEEDED: IP {IP} made {Count} requests in the last minute (limit: {Limit}) | Path: {Path} | UserAgent: {UserAgent}",
                            clientIp,
                            _ipRequestCounts[clientIp],
                            _maxRequestsPerMinute,
                            context.Request.Path,
                            context.Request.Headers.UserAgent.ToString().Truncate(100)
                        );
                    }
                }
                else
                {
                    _ipRequestCounts[clientIp] = 1;
                }
            }
            else
            {
                _ipRequestCounts[clientIp] = 1;
            }

            _ipLastRequest[clientIp] = now;
        }

        private void LogAuthenticationInfo(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var roles = context.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();

                var userId = context.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                _logger.LogInformation(
                    "👤 AUTHENTICATED REQUEST: User: {User} | UserId: {UserId} | {Method} {Path} | Roles: [{Roles}] | IP: {IP}",
                    context.User.Identity.Name,
                    userId,
                    context.Request.Method,
                    context.Request.Path,
                    string.Join(", ", roles),
                    GetClientIP(context)
                );

                // Log privileged operations
                if (roles.Contains("Admin") || context.Request.Path.StartsWithSegments("/Admin"))
                {
                    _logger.LogWarning(
                        "🔐 ADMIN ACCESS: User: {User} accessing {Method} {Path} | IP: {IP} | UserAgent: {UserAgent}",
                        context.User.Identity.Name,
                        context.Request.Method,
                        context.Request.Path,
                        GetClientIP(context),
                        context.Request.Headers.UserAgent.ToString().Truncate(100)
                    );
                }
            }
        }

        private void LogSuspiciousActivity(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();
            var queryString = context.Request.QueryString.ToString().ToLower();
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var clientIp = GetClientIP(context);

            // Check for SQL injection attempts
            var sqlInjectionPatterns = new[]
            {
                "union select", "drop table", "insert into", "delete from", "update set",
                "exec(", "execute(", "sp_", "xp_", "'; --", "' or '1'='1", "' or 1=1",
                "/*", "*/", "@@", "char(", "nchar(", "varchar(", "nvarchar(",
                "waitfor delay", "benchmark(", "pg_sleep("
            };

            if (sqlInjectionPatterns.Any(pattern => path.Contains(pattern) || queryString.Contains(pattern)))
            {
                _logger.LogError(
                    "💉 SQL INJECTION ATTEMPT detected: {Method} {Path}{Query} | IP: {IP} | UserAgent: {UserAgent}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString,
                    clientIp,
                    userAgent.Truncate(200)
                );
            }

            // Check for XSS attempts
            var xssPatterns = new[]
            {
                "<script", "javascript:", "vbscript:", "onload=", "onerror=", "onclick=",
                "eval(", "expression(", "url(javascript:", "mocha:", "livescript:",
                "<%", "%>", "${", "{{", "}}"
            };

            if (xssPatterns.Any(pattern => path.Contains(pattern) || queryString.Contains(pattern)))
            {
                _logger.LogError(
                    "💻 XSS ATTEMPT detected: {Method} {Path}{Query} | IP: {IP} | UserAgent: {UserAgent}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString,
                    clientIp,
                    userAgent.Truncate(200)
                );
            }

            // Check for path traversal attempts
            var pathTraversalPatterns = new[] { "../", "..\\", "%2e%2e%2f", "%2e%2e%5c", "..%2f", "..%5c" };
            if (pathTraversalPatterns.Any(pattern => path.Contains(pattern) || queryString.Contains(pattern)))
            {
                _logger.LogError(
                    "📁 PATH TRAVERSAL ATTEMPT detected: {Method} {Path}{Query} | IP: {IP} | UserAgent: {UserAgent}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString,
                    clientIp,
                    userAgent.Truncate(200)
                );
            }

            // Check for bot activity
            var botPatterns = new[] { "bot", "crawler", "spider", "scraper", "wget", "curl" };
            if (botPatterns.Any(pattern => userAgent.ToLower().Contains(pattern)) ||
                string.IsNullOrEmpty(userAgent) || userAgent.Length < 10)
            {
                _logger.LogInformation(
                    "🤖 BOT ACTIVITY detected: UserAgent: {UserAgent} | IP: {IP} | Path: {Path}",
                    userAgent.Truncate(100),
                    clientIp,
                    context.Request.Path
                );
            }

            // Check for suspicious file access attempts
            var suspiciousFiles = new[]
            {
                ".env", ".git", "web.config", "appsettings.json", ".htaccess",
                "wp-config.php", "admin.php", "phpinfo.php", "shell.php"
            };

            if (suspiciousFiles.Any(file => path.Contains(file)))
            {
                _logger.LogWarning(
                    "📋 SUSPICIOUS FILE ACCESS attempt: {Method} {Path} | IP: {IP} | UserAgent: {UserAgent}",
                    context.Request.Method,
                    context.Request.Path,
                    clientIp,
                    userAgent.Truncate(100)
                );
            }
        }

        private void LogSecurityEvents(HttpContext context, string clientIp)
        {
            var statusCode = context.Response.StatusCode;

            // Log unauthorized access attempts
            if (statusCode == 401)
            {
                _logger.LogWarning(
                    "🔒 UNAUTHORIZED ACCESS attempt: {Method} {Path} | IP: {IP} | UserAgent: {UserAgent} | Referer: {Referer}",
                    context.Request.Method,
                    context.Request.Path,
                    clientIp,
                    context.Request.Headers.UserAgent.ToString().Truncate(100),
                    context.Request.Headers.Referer.ToString()
                );
            }

            // Log forbidden access attempts
            if (statusCode == 403)
            {
                _logger.LogWarning(
                    "🚫 FORBIDDEN ACCESS attempt: {Method} {Path} | User: {User} | IP: {IP} | UserAgent: {UserAgent}",
                    context.Request.Method,
                    context.Request.Path,
                    context.User?.Identity?.Name ?? "Anonymous",
                    clientIp,
                    context.Request.Headers.UserAgent.ToString().Truncate(100)
                );
            }

            // Log successful admin actions
            if (context.User?.Identity?.IsAuthenticated == true &&
                context.User.IsInRole("Admin") &&
                statusCode >= 200 && statusCode < 300 &&
                (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "DELETE"))
            {
                _logger.LogInformation(
                    "⚙️ ADMIN ACTION completed: {User} performed {Method} {Path} | Status: {StatusCode} | IP: {IP}",
                    context.User.Identity.Name,
                    context.Request.Method,
                    context.Request.Path,
                    statusCode,
                    clientIp
                );
            }

            // Log failed login attempts (if this is a login endpoint)
            if (context.Request.Path.StartsWithSegments("/Account/Login") ||
                context.Request.Path.StartsWithSegments("/Identity/Account/Login"))
            {
                if (statusCode != 200 && context.Request.Method == "POST")
                {
                    _logger.LogWarning(
                        "🔑 FAILED LOGIN ATTEMPT: IP: {IP} | UserAgent: {UserAgent} | Status: {StatusCode}",
                        clientIp,
                        context.Request.Headers.UserAgent.ToString().Truncate(100),
                        statusCode
                    );
                }
                else if (statusCode == 200 && context.Request.Method == "POST")
                {
                    _logger.LogInformation(
                        "✅ SUCCESSFUL LOGIN: User: {User} | IP: {IP} | UserAgent: {UserAgent}",
                        context.User?.Identity?.Name ?? "Unknown",
                        clientIp,
                        context.Request.Headers.UserAgent.ToString().Truncate(100)
                    );
                }
            }
        }

        private static string GetClientIP(HttpContext context)
        {
            return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim() ??
                   context.Request.Headers["X-Real-IP"].FirstOrDefault() ??
                   context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? // Cloudflare
                   context.Request.Headers["True-Client-IP"].FirstOrDefault() ??   // Cloudflare Enterprise
                   context.Connection.RemoteIpAddress?.ToString() ??
                   "Unknown";
        }
    }
}