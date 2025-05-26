using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VaccinationCardManagement.Application.Behaviors;
using VaccinationCardManagement.Application.Services.Authentication;
using VaccinationCardManagement.Application.Services.User;
using VaccinationCardManagement.Database;

namespace VaccinationCardManagement.Application;

public static class ApplicationModuleDependency
{
    public static void AddApplicationModuleDependency(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddDataBaseModuleDependency(configuration);

        //Register service
        services.AddScoped<AuthenticationService>();
        services.AddScoped<UserService>();
    }
}
