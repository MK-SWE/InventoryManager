using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<ProductStock> ProductStocks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<BaseEntity>().HasQueryFilter(e => !e.IsDeleted);
        
        modelBuilder.Entity<Product>().ToTable("Products").HasQueryFilter(e => e.IsDeleted != true);
        modelBuilder.Entity<Product>(entity => 
        {
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .HasDefaultValue(Array.Empty<byte>());
            entity.HasIndex(p => p.SKU).IsUnique();
        });
        
        modelBuilder.Entity<Warehouse>().ToTable("Warehouses").HasQueryFilter(e => e.IsDeleted != true);
        modelBuilder.Entity<Warehouse>(entity => 
        {
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .HasDefaultValue(Array.Empty<byte>());
            entity.HasIndex(w => w.WarehouseCode).IsUnique();
        });
        
        modelBuilder.Entity<ProductStock>().ToTable("ProductStocks").HasQueryFilter(e => e.IsDeleted != true);
        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity.HasIndex(ps => new { ps.ProductId, ps.WarehouseId }).IsUnique();
            
            entity.HasOne(ps => ps.Product)
                .WithMany(p => p.ProductStocks)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ps => ps.Warehouse)
                .WithMany(w => w.ProductStocks)
                .HasForeignKey(ps => ps.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .HasDefaultValue(Array.Empty<byte>());
        });
    }
}