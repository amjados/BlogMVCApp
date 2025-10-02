using Microsoft.Extensions.Caching.Memory;

namespace BlogMVCApp.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class;
        Task RemoveAsync(string key);
        Task RemovePatternAsync(string pattern);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;
        private readonly HashSet<string> _cacheKeys;

        public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _cacheKeys = new HashSet<string>();
        }

        public Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                var result = _memoryCache.Get<T>(key);
                if (result != null)
                {
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                }
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cache value for key: {Key}", key);
                return Task.FromResult<T?>(null);
            }
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            try
            {
                var options = new MemoryCacheEntryOptions();

                if (expiration.HasValue)
                {
                    options.SetAbsoluteExpiration(expiration.Value);
                }
                else
                {
                    // Default expiration of 30 minutes
                    options.SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                }

                // Set size for the cache entry (required when SizeLimit is configured)
                options.Size = EstimateCacheEntrySize(value);

                // Add callback to remove from tracking when evicted
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    if (key is string stringKey)
                    {
                        lock (_cacheKeys)
                        {
                            _cacheKeys.Remove(stringKey);
                        }
                    }
                });

                _memoryCache.Set(key, value, options);

                lock (_cacheKeys)
                {
                    _cacheKeys.Add(key);
                }

                _logger.LogDebug("Cache set for key: {Key} with expiration: {Expiration}", key, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache value for key: {Key}", key);
            }

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            try
            {
                _memoryCache.Remove(key);
                lock (_cacheKeys)
                {
                    _cacheKeys.Remove(key);
                }
                _logger.LogDebug("Cache removed for key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache value for key: {Key}", key);
            }

            return Task.CompletedTask;
        }

        public Task RemovePatternAsync(string pattern)
        {
            try
            {
                List<string> keysToRemove;
                lock (_cacheKeys)
                {
                    keysToRemove = _cacheKeys.Where(k => k.Contains(pattern)).ToList();
                }

                foreach (var key in keysToRemove)
                {
                    _memoryCache.Remove(key);
                    lock (_cacheKeys)
                    {
                        _cacheKeys.Remove(key);
                    }
                }

                _logger.LogDebug("Cache removed for pattern: {Pattern}, removed {Count} keys", pattern, keysToRemove.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache values for pattern: {Pattern}", pattern);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Estimates the size of a cache entry for memory management
        /// </summary>
        /// <param name="value">The value to estimate size for</param>
        /// <returns>Estimated size in units</returns>
        private static long EstimateCacheEntrySize<T>(T value)
        {
            if (value == null) return 1;

            return value switch
            {
                byte[] bytes => Math.Max(1, bytes.Length / 1024), // 1 unit per KB
                string str => Math.Max(1, str.Length / 100), // Rough estimate: 100 chars = 1 unit
                System.Collections.ICollection collection => Math.Max(1, collection.Count), // 1 unit per item
                _ => 1 // Default size for other types
            };
        }
    }
}