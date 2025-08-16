using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<ProductStock> ProductStocks { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    public DbSet<InventoryTransactionHeader> InventoryTransactionHeaders { get; set; }
    public DbSet<InventoryTransactionLine> InventoryTransactionLines { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model
                     .GetEntityTypes()
                     .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.RowVersion))
                .IsRowVersion()
                .HasDefaultValue(Array.Empty<byte>());
        }

        #region Database Tables Definetion
        
            modelBuilder.Entity<Product>(entity => 
            {
                entity.ToTable("Products").HasQueryFilter(e => e.IsDeleted != true);
                entity.HasIndex(p => p.SKU).IsUnique();
            });
            
            modelBuilder.Entity<Warehouse>(entity => 
            {
                entity.ToTable("Warehouses").HasQueryFilter(e => e.IsDeleted != true);
                entity.HasIndex(w => w.WarehouseCode).IsUnique();
            });
            
            modelBuilder.Entity<ProductStock>(entity =>
            {
                entity.ToTable("ProductStocks").HasQueryFilter(e => e.IsDeleted != true);
                entity.HasIndex(ps => new { ps.ProductId, ps.WarehouseId }).IsUnique();
                
                entity.HasOne(ps => ps.Product)
                    .WithMany(p => p.ProductStocks)
                    .HasForeignKey(ps => ps.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(ps => ps.Warehouse)
                    .WithMany(w => w.ProductStocks)
                    .HasForeignKey(ps => ps.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories").HasQueryFilter(category => !category.IsDeleted);
            });
        
            modelBuilder.Entity<UnitOfMeasure>(entity =>
            {
                entity.ToTable("UnitOfMeasures").HasQueryFilter(uom => !uom.IsDeleted);
            });

            modelBuilder.Entity<InventoryTransactionHeader>(entity =>
            {
                entity.ToTable("InventoryTransactionHeaders").HasQueryFilter(header => !header.IsDeleted);
                
                entity.HasMany(header => header.Lines)
                    .WithOne(line => line.Header)
                    .HasForeignKey(line => line.TransactionHeaderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InventoryTransactionLine>(entity =>
            {
                entity.ToTable("InventoryTransactionLines").HasQueryFilter(line => !line.IsDeleted);
                
                entity.Property(line => line.UnitCost).HasColumnType("decimal(18,4)");
            });
        
        #endregion
    }
}