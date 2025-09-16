using FluentValidation;
using Inventory.Application.Common.Behaviors;
using Inventory.Application.Common.HelpingMethods;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Common.Mapping;
using Inventory.Application.InventoryStock.Services;
using Inventory.Application.Products.Validators;
using Inventory.Application.StockReservation.Services;
using Inventory.Application.Transactions.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Repositories;
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
            cfg.AddProfile<ApplicationMappingProfile>();
        });

        services.AddScoped<ProductValidationHelper>();
        services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
        services.AddScoped<IInventoryStockService, InventoryStockService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IInventoryStockReservationService, InventoryStockReservationService>();
        services.AddScoped<IInventoryStockReservationRepository, InventoryStockReservationRepositoryRepository>();
        
        return services;
    }
}