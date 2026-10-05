using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using VendorHub.Application.Common.Interfaces;

namespace VendorHub.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                var cachedData = await _cache.GetStringAsync(key, cancellationToken);
                if (string.IsNullOrEmpty(cachedData))
                    return default;

                return JsonSerializer.Deserialize<T>(cachedData);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis error while retrieving key {CacheKey}. Falling back.", key);                                                                    
                return default;                              
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan?  expiration = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
                };

                var serialized = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(key, serialized, options, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis error while setting key {CacheKey}.", key);
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis error while removing key {CacheKey}.", key);
            }
        }
    }
}
