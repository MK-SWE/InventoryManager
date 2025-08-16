using FluentValidation;
using Inventory.API.Filters;
using Inventory.API.Mapping;

namespace Inventory.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // API-specific services
        services.AddControllers(options =>
        {
            options.Filters.Add<FluentValidationFilter>();
        });
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ApiMappingProfile>();
        });
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Inventory.API.xml"));
        });
        
        return services;
    }
}