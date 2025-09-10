using System.Text.Json;
using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Repository.V1;
using Back_Almazara.Utility;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Back_Almazara.Service.V1
{

    public interface IRedisCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data, int expirationMinutes = 10);
        Task SetWithTagsAsync<T>(string key, T data, string[] tags, int expirationMinutes = 10);
        Task RemoveAsync(string key);
        Task InvalidateByTagAsync(string tag);
        Task<bool> ExistsAsync(string key);
    }

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<RedisCacheService> _logger;
        private const int DEFAULT_CACHE_MINUTES = 10;

        public RedisCacheService(
            IDistributedCache cache,
            IConnectionMultiplexer connectionMultiplexer,
            ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var jsonData = await _cache.GetStringAsync(key);
                if (jsonData == null)
                {
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                    return default;
                }

                _logger.LogDebug("Cache hit for key: {Key}", key);
                return JsonSerializer.Deserialize<T>(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cache for key: {Key}", key);
                return default; // Graceful degradation
            }
        }

        public async Task SetAsync<T>(string key, T data, int expirationMinutes = DEFAULT_CACHE_MINUTES)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(expirationMinutes / 2)
                };

                var serializedData = JsonSerializer.Serialize(data);
                await _cache.SetStringAsync(key, serializedData, options);

                _logger.LogDebug("Cache set for key: {Key} with expiration: {Minutes} minutes", key, expirationMinutes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache for key: {Key}", key);
                // No relanzar la excepción para evitar interrumpir el flujo principal
            }
        }

        public async Task SetWithTagsAsync<T>(string key, T data, string[] tags, int expirationMinutes = DEFAULT_CACHE_MINUTES)
        {
            try
            {
                // Primero establecer el cache principal
                await SetAsync(key, data, expirationMinutes);

                // Luego asociar con tags usando ConnectionMultiplexer
                var database = _connectionMultiplexer.GetDatabase();
                var tasks = tags.Select(async tag =>
                {
                    var tagKey = $"tag:{tag}";
                    await database.SetAddAsync(tagKey, key);
                    await database.KeyExpireAsync(tagKey, TimeSpan.FromMinutes(expirationMinutes + 5));
                });

                await Task.WhenAll(tasks);

                _logger.LogDebug("Cache set with tags for key: {Key}, tags: {Tags}", key, string.Join(", ", tags));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache with tags for key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogDebug("Cache removed for key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache for key: {Key}", key);
            }
        }

        public async Task InvalidateByTagAsync(string tag)
        {
            try
            {
                var database = _connectionMultiplexer.GetDatabase();
                var tagKey = $"tag:{tag}";

                var keys = await database.SetMembersAsync(tagKey);
                if (keys.Length > 0)
                {
                    // Eliminar las claves del cache distribuido
                    var removeTasks = keys.Select(key => _cache.RemoveAsync(key.ToString()));
                    await Task.WhenAll(removeTasks);

                    // Eliminar también de Redis directamente
                    await database.KeyDeleteAsync(keys.Select(k => (RedisKey)k.ToString()).ToArray());

                    _logger.LogInformation("Invalidated {Count} keys for tag: {Tag}", keys.Length, tag);
                }

                // Eliminar la etiqueta
                await database.KeyDeleteAsync(tagKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating cache by tag: {Tag}", tag);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(key);
                return value != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking cache existence for key: {Key}", key);
                return false;
            }
        }
    }
}
