namespace VendorHub.Application;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VendorHub.Application.Common.Behaviors;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);

            // Register pipeline behaviors in execution order
            cfg.AddOpenBehavior(typeof(RequestPerformanceBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
