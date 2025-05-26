using LazyCache;
using VaccinationCardManagement.Cache.Cache.Extensions;

namespace VaccinationCardManagement.Cache.Cache;

public class CacheProviderMemory : Interface.ICacheProvider
{
    private readonly IAppCache _cache;

    public CacheProviderMemory(IAppCache cache)
    {
        _cache = cache;
    }

    private List<string> GetAllKeys()
    {
        var result = _cache.GetKeys().ToList();
        return result;
    }

    public async Task ClearCacheAsync()
    {
        var results = _cache.GetKeys();
        foreach (var key in results)
        {
            _cache.Remove(key);
        }
    }

    public async Task ClearCacheByKeyAsync(string key)
    {
        _cache.Remove(key);
    }

    public async Task<List<string>> GetAllKeysInCache()
    {
        return GetAllKeys();
    }

    public async Task<T> GetFromCacheAsync<T>(string key) where T : class
    {
        var result = await _cache.GetAsync<T>(key);
        return result;
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration) where T : class
    {
        var time = DateTimeOffset.UtcNow.AddHours(expiration.Hours).AddMinutes(expiration.Minutes).AddSeconds(expiration.Seconds);
       _cache.Add<T>(key, value, time);
    }
}
