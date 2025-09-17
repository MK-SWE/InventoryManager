using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<ProductStock> ProductStocks { get; set; }
    public DbSet<InventoryStockReservation> InventoryStockReservation { get; set; }
    public DbSet<InventoryStockReservationLine> InventoryStockReservationLine { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    public DbSet<InventoryTransaction> InventoryTransactionHeaders { get; set; }
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
                entity.ToTable("Products").HasQueryFilter(e => !e.IsDeleted);
                entity.HasIndex(p => p.SKU).IsUnique();
            });
            
            modelBuilder.Entity<Warehouse>(entity => 
            {
                entity.ToTable("Warehouses").HasQueryFilter(e => !e.IsDeleted);
                entity.HasIndex(warehouse => warehouse.WarehouseCode).IsUnique();
                entity.OwnsOne(warehouse => warehouse.WarehouseAddress, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.Property(address => address.Line1);
                    ownedNavigationBuilder.Property(address => address.Line2);
                    ownedNavigationBuilder.Property(address => address.City);
                    ownedNavigationBuilder.Property(address => address.State);
                    ownedNavigationBuilder.Property(address => address.PostalCode);
                    ownedNavigationBuilder.Property(address => address.Country);
                });
            });
            
            modelBuilder.Entity<ProductStock>(entity =>
            {
                entity.ToTable("ProductStocks").HasQueryFilter(e => !e.IsDeleted);
                entity.HasIndex(ps => new { ps.ProductId, ps.WarehouseId }).IsUnique();
                
                entity.HasOne(ps => ps.Product)
                    .WithMany(p => p.ProductStocks)
                    .HasForeignKey(ps => ps.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(ps => ps.Warehouse)
                    .WithMany(w => w.ProductStocks)
                    .HasForeignKey(ps => ps.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.OwnsOne(ps => ps.StockStatus, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.Property(status => status.AvailableStock);
                    ownedNavigationBuilder.Property(status => status.OnHoldStock);
                    ownedNavigationBuilder.Property(status => status.QuarantinedStock);
                    ownedNavigationBuilder.Property(status => status.QualityControlStock);
                    ownedNavigationBuilder.Property(status => status.ReturnedStock);
                    ownedNavigationBuilder.Property(status => status.DamagedStock);
                });
            });
            
            modelBuilder.Entity<InventoryStockReservation>(entity =>
            {
                entity.ToTable("InventoryStockReservation").HasQueryFilter(e => !e.IsDeleted);
                
                entity.HasKey(reservation => reservation.Id);
                entity.HasIndex(reservation => reservation.ReservationReference).IsUnique();
                entity.HasMany(reservation => reservation.ReservationLines)
                    .WithOne(line => line.Reservation)
                    .HasForeignKey(line => line.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Metadata.FindNavigation(nameof(InventoryStockReservationLine))
                    ?.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<InventoryStockReservationLine>(entity =>
            {
                entity.ToTable("InventoryStockReservationLine").HasQueryFilter(line => !line.IsDeleted);
                entity.HasIndex(resLine => new { resLine.ReservationId, resLine.ProductId }).IsUnique();
                
                entity.HasOne(line => line.Reservation)
                    .WithMany(reservation => reservation.ReservationLines)
                    .HasForeignKey(line => line.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);
        
                entity.HasOne(line => line.Product)
                    .WithMany(product => product.Reservations)
                    .HasForeignKey(line => line.ProductId)
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

            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.ToTable("InventoryTransactionHeaders").HasQueryFilter(header => !header.IsDeleted);
                
                entity.HasIndex(e => e.ReferenceNumber).IsUnique();
                entity.HasKey(e => e.Id);
                
                entity.HasMany(e => e.Lines)
                    .WithOne(e => e.Header)
                    .HasForeignKey(e => e.TransactionHeaderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(t => t.SourceWarehouseId).IsRequired(false);
                entity.Property(t => t.DestinationWarehouseId).IsRequired(false);
            });

            modelBuilder.Entity<InventoryTransactionLine>(entity =>
            {
                entity.ToTable("InventoryTransactionLines").HasQueryFilter(line => !line.IsDeleted);

                entity.HasKey(e => e.Id);
                entity.HasOne(line => line.Product)
                    .WithMany()
                    .HasForeignKey(line => line.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(line => line.UnitCost).HasColumnType("decimal(18,4)");
            });
        
        #endregion
    }
}