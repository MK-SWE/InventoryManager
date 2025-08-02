using FluentValidation;
using Inventory.Application.Common.Behaviors;
using Inventory.Application.Common.Mapping;
using Inventory.Application.Products.Validators;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Get the assembly reference
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
    
        // Register behavior as transient
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // MediatR with assembly scanning
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(assembly));

        // AutoMapper with assembly scanning
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        services.AddScoped<ProductValidationHelper>();
        
        return services;
    }
}