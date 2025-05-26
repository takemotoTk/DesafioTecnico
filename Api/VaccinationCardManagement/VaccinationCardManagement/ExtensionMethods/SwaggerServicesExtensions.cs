using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace VaccinationCardManagement.ExtensionMethods;

public static class SwaggerServicesExtensions
{
    public static void AddSwaggerService(this IServiceCollection services
        , IConfiguration configuration
        , string titleInfo
        , int apiVersion = 1
        , string description = null
        , bool includeXmlControllersComments = false)
    {
        services.AddSwaggerGen(c =>
        {
            c.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("0.00:00:00.000")
            });
            ConfigureSwaggerDoc(c, titleInfo, description, apiVersion);
            if (includeXmlControllersComments)
                AddXmlControllersComments(c);
        });
    }

    public static void ConfigureSwaggerApplication(this IApplicationBuilder app, IWebHostEnvironment env = null, bool useCustomTopPage = false, int apiVersion = 1, string route = "api/help")
    {
        app.UseSwagger(o =>
        {
            o.RouteTemplate = $"{route}/{{documentName}}/docs.json";
        });

        app.UseSwaggerUI(options =>
        {
            if (useCustomTopPage && env != null)
            {
                var topBar = Path.Combine(env.ContentRootPath, "wwwroot/swagger-ui/index.html");
                options.HeadContent = File.ReadAllText(topBar);
                options.InjectStylesheet("/swagger-ui/custom.css");
            }

            options.RoutePrefix = route;
            options.SwaggerEndpoint($"/{route}/v{apiVersion}/docs.json", $"Version {apiVersion}");
        });
        app.UseStaticFiles();
    }

    public static void ConfigureSwaggerSecurityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwaggerGen(options =>
        {
            //add JWT Authentication
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, new string[] { }}
            });
        });
    }

    private static void ConfigureSwaggerDoc(SwaggerGenOptions options, string titleInfo, string description, int apiVersion = 1)
    {
        var apiVersionString = $"v{apiVersion}";
        description ??= titleInfo;
        options.SwaggerDoc(apiVersionString, new OpenApiInfo
        {
            Title = titleInfo,
            Version = apiVersionString,
            Description = description,
        });
    }

    private static void AddXmlControllersComments(SwaggerGenOptions options)
    {
        var xmlPath = AppContext.BaseDirectory;
        var xmlFile = Path.Combine(xmlPath, $"{Assembly.GetEntryAssembly().GetName().Name}.xml");

        Console.WriteLine($"XML comment file: {xmlFile}");

        if (File.Exists(xmlFile))
            options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
    }
}
