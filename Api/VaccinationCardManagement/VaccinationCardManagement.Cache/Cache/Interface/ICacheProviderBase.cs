namespace VaccinationCardManagement.Cache.Cache.Interface;

public interface ICacheProviderBase
{
    Task<T> GetFromCacheAsync<T>(string key) where T : class;
    Task SetCacheAsync<T>(string key, T value, TimeSpan expiration) where T : class;
    Task ClearCacheByKeyAsync(string key);
}