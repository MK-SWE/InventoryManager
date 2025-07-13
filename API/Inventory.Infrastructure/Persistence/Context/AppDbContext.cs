using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }
    // Add DbSet properties here
    // public DbSet<Item> Items { get; set; }
}