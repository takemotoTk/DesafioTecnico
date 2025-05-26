namespace VaccinationCardManagement.Domain.Adapter;

public interface ICacheManager
{
    Task<T> GetFromCacheAsync<T>(string key) where T : class;
    Task<List<string>> GetAllKeysInCache();
    Task SetCacheAsync<T>(string key, T value, TimeSpan expiration) where T : class;
    Task ClearCacheByKeyAsync(string key);
    Task ClearCacheAsync();
}