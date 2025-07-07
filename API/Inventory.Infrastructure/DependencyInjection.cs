using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repository registrations
        services.AddScoped<IReadRepository<Product>, ProductRepository>();
        services.AddScoped<IWriteRepository<Product>, ProductRepository>();
        
        // Add other repositories similarly:
        // services.AddScoped<IReadRepository<Warehouse>, WarehouseRepository>();
        // services.AddScoped<IWriteRepository<Warehouse>, WarehouseRepository>();
        
        return services;
    }
}