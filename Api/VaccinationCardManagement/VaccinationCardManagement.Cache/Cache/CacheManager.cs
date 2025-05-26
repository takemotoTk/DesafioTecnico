using VaccinationCardManagement.Cache.Cache.Interface;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Cache.Cache;

public class CacheManager : ICacheManager
{
    private readonly ICacheProvider _cache;
    public CacheManager(ICacheProvider cache)
    {
        _cache = cache;
    }

    public async Task<T> GetFromCacheAsync<T>(string key) where T : class
    {
        return await _cache.GetFromCacheAsync<T>(key);
    }

    public async Task<List<string>> GetAllKeysInCache()
    {
        return await _cache.GetAllKeysInCache();
    }
    public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration) where T : class
    {
        await _cache.SetCacheAsync(key, value, expiration);
    }

    public async Task ClearCacheByKeyAsync(string key)
    {
        await _cache.ClearCacheByKeyAsync(key);
    }

    public async Task ClearCacheAsync()
    {
        await _cache.ClearCacheAsync();
    }
}