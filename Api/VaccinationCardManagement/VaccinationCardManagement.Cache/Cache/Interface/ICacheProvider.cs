namespace VaccinationCardManagement.Cache.Cache.Interface;

public interface ICacheProvider : ICacheProviderBase
{
    Task ClearCacheAsync();
    Task<List<string>> GetAllKeysInCache();
}