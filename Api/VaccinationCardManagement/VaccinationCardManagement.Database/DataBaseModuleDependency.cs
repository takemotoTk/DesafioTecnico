using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaccinationCardManagement.Cache;
using VaccinationCardManagement.Database.Context;
using VaccinationCardManagement.Database.Repository;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Database;

public static class DataBaseModuleDependency
{
    public static void AddDataBaseModuleDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCacheModuleDependency();

        var connectionString = configuration.GetConnectionString("Database");
        //PostGree
        services.AddDbContext<VaccinationCardManagementContext>(options =>
        {
            options.UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, ServiceLifetime.Scoped);

        services.AddScoped<IVaccinationCardManagementRepository, VaccinationCardManagementRepository>();
    }
}