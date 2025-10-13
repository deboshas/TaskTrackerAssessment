using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskTracker.Infrastructure.Persistance;
using TaskTracker.Infrastructure.Repositories;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application;

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
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        return services
               .AddPersitance(configuration, hostEnvironment)
               .ConfigureRepositories();
    }

    /// <summary>  
    /// Configures and registers the database context for persistence using Entity Framework Core.  
    /// </summary>  
    /// <param name="services">The service collection to which the database context will be added.</param>  
    /// <param name="configuration">The application configuration containing the database connection string.</param>  
    /// <param name="hostEnvironment">The hosting environment of the application.</param>  
    /// <returns>The updated service collection with the registered database context.</returns>  
    private static IServiceCollection AddPersitance(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        var connectionString = configuration.GetSection("Database:ConnectionString").Value ?? throw new ArgumentNullException(nameof(configuration));

        return services.AddDbContext<TaskTrackerDbContext>(options =>
        {
            options.UseSqlServer(connectionString, o =>
            {
                o.EnableRetryOnFailure(3);
            });
            if (hostEnvironment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
    }

    /// <summary>  
    /// Configures and registers repository services for dependency injection.  
    /// </summary>  
    /// <param name="services">The service collection to which repository services will be added.</param>  
    /// <returns>The updated service collection with registered repository services.</returns>  
    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        return services.AddScoped<ITaskTrackerRepository, TaskTrackerRepository>();
    }
}
