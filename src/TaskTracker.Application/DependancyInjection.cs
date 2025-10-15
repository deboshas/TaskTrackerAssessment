using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TaskTracker.Application.CQRS.Behaviours;
using TaskTracker.Application.Task.Create;

namespace TaskTracker.Application;

[ExcludeFromCodeCoverage]
public static class DependancyInjection
{
    /// <summary>
    /// Registers application services including MediatR and validators.
    /// </summary>
    /// <param name="services">The service collection to register services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .ConfigureMediatR()
            .ConfigureValidators();
    }

    private static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        return services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTaskCommand>())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }

    private static IServiceCollection ConfigureValidators(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssembly(typeof(DependancyInjection).Assembly);
    }
}
