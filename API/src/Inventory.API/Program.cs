using Inventory.API.Extensions;
using Inventory.Application;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Layer-based service registration
builder.Services
    .AddApiServices(builder.Configuration)    // API Layer
    .AddApplication()                         // Application Layer
    .AddInfrastructure(builder.Configuration); // Infrastructure Layer

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAll", builder => 
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var app = builder.Build();

// Database migration and seeding
await using (var scope = app.Services.CreateAsyncScope())
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
            await AppDbContextSeed.SeedAsync(context, logger);
            
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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();