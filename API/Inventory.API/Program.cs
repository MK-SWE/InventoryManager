using Inventory.Application.Common.Mapping;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Infrastructure.Persistence.Repositories;
using Inventory.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IReadRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IWriteRepository<Product>, ProductRepository>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblies(
        typeof(Inventory.Application.Commands.CreateProductCommand).Assembly,
        typeof(Program).Assembly
    )
);
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        // Apply pending migrations
        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");

        // Begin transaction for seeding
        logger.LogInformation("Starting database seeding...");
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            // Seed initial data within the transaction
            await AppDbContextSeed.SeedAsync(context);
            
            // Commit transaction if successful
            await transaction.CommitAsync();
            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception seedEx)
        {
            // Rollback transaction on seeding failure
            await transaction.RollbackAsync();
            logger.LogError(seedEx, "Seeding failed. Rolled back seed changes.");
            throw; // Re-throw to outer catch block
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during database initialization");
        // For production: Consider if you want to stop the application
        // throw; // Uncomment to terminate application on startup failure
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();