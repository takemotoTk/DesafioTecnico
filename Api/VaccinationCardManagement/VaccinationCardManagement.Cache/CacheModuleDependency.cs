using LazyCache.Providers;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VaccinationCardManagement.Domain.Adapter;
using VaccinationCardManagement.Cache.Cache;

namespace VaccinationCardManagement.Cache;

public static class CacheModuleDependency
{
    public static void AddCacheModuleDependency(this IServiceCollection services)
    {
        //Cache implementation generic
        services.AddScoped<ICacheManager, CacheManager>();

        //Cache Memory
        services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
        services.TryAdd(ServiceDescriptor.Singleton<ICacheProvider, MemoryCacheProvider>());
        services.TryAdd(ServiceDescriptor.Singleton<IAppCache, CachingService>(serviceProvider =>
            new CachingService(
                new Lazy<ICacheProvider>(serviceProvider.GetRequiredService<ICacheProvider>))));
        services.AddScoped<Cache.Interface.ICacheProvider, CacheProviderMemory>();
    }
}
