namespace Inventory.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // API-specific services
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Inventory.API.xml"));
        });
        
        return services;
    }
}