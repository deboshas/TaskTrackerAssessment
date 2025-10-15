using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using TaskTracker.API.Middleware;

namespace TaskTracker.API;

[ExcludeFromCodeCoverage]
public static class DependancyInjection
{
    /// <summary>  
    /// Configures and registers infrastructure services, including persistence and repositories, into the service collection.  
    /// </summary>  
    /// <param name="services">The service collection to which services will be added.</param>  
    /// <param name="configuration">The application configuration containing necessary settings.</param>  
    /// <param name="hostEnvironment">The hosting environment of the application.</param>  
    /// <returns>The updated service collection with registered infrastructure services.</returns>  
    public static IServiceCollection AddWebAppServices(this IServiceCollection services)
    {
        services
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TaskTracker API",
                    Version = "v1",
                    Description = "TaskTracker API with Swagger"
                });
            });

        return services.AddScoped<ValidationExceptionMiddleware>();
    }

    public static WebApplication UseSwaggerMiddleware(this WebApplication app)
    {

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasks API v1");
            c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        });

        return app;
    }

}
