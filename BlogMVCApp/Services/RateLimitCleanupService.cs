using BlogMVCApp.Filters;

namespace BlogMVCApp.Services;

/// <summary>
/// Background service to clean up rate limiting data periodically
/// </summary>
public class RateLimitCleanupService : BackgroundService
{
    private readonly ILogger<RateLimitCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5); // Clean up every 5 minutes

    public RateLimitCleanupService(ILogger<RateLimitCleanupService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🧹 Rate Limit Cleanup Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogDebug("🧹 Performing rate limit cleanup...");
                RateLimitAttribute.Cleanup();
                _logger.LogDebug("✅ Rate limit cleanup completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error during rate limit cleanup");
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }

        _logger.LogInformation("🧹 Rate Limit Cleanup Service stopped");
    }
}