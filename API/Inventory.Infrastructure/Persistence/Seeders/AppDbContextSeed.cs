using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Inventory.Infrastructure.Persistence.Context;


namespace Inventory.Infrastructure.Persistence.Seeders;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Seed Products
        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new Product {
                    SKU = "ELEC-001",
                    ProductName = "Wireless Mouse",
                    ProductDescription = "Ergonomic wireless mouse with 2.4GHz connectivity",
                    CategoryId = 1,
                    UnitOfMeasureId = 1,
                    UnitPrice = 19.99m,
                    ReorderLevel = 50,
                    Weight = 120,
                    Volume = 150,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Product {
                    SKU = "OFFICE-100",
                    ProductName = "Desk Lamp",
                    ProductDescription = "LED desk lamp with adjustable brightness",
                    CategoryId = 2,
                    UnitOfMeasureId = 1,
                    UnitPrice = 34.50m,
                    ReorderLevel = 30,
                    Weight = 850,
                    Volume = 1200,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Product {
                    SKU = "KIT-550",
                    ProductName = "Ceramic Coffee Mug",
                    ProductDescription = "350ml ceramic mug with heat-resistant handle",
                    CategoryId = 3,
                    UnitOfMeasureId = 2,
                    UnitPrice = 8.75m,
                    ReorderLevel = 100,
                    Weight = 350,
                    Volume = 400,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Product {
                    SKU = "CLO-2200",
                    ProductName = "Cotton T-Shirt",
                    ProductDescription = "Premium cotton crew neck t-shirt",
                    CategoryId = 4,
                    UnitOfMeasureId = 3,
                    UnitPrice = 24.99m,
                    ReorderLevel = 75,
                    Weight = 180,
                    Volume = 300,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Product {
                    SKU = "ELEC-045",
                    ProductName = "Bluetooth Earbuds",
                    ProductDescription = "True wireless earbuds with charging case",
                    CategoryId = 1,
                    UnitOfMeasureId = 4,
                    UnitPrice = 59.99m,
                    ReorderLevel = 25,
                    Weight = 50,
                    Volume = 80,
                    IsActive = false,
                    CreatedDate = DateTime.UtcNow
                }
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
        
        if (!await context.Warehouses.AnyAsync())
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse {
                    WarehouseName = "Showroom Warehouse",
                    WarehouseCode = "Main-123",
                    WarehouseAddress = "123 main st",
                    IsActive = true,
                    Capacity = 123,
                    CreatedDate = DateTime.UtcNow
                },
                new Warehouse {
                    WarehouseName = "Secondary Warehouse",
                    WarehouseCode = "Second-1234",
                    WarehouseAddress = "321 secondary st",
                    IsActive = true,
                    Capacity = 500,
                    CreatedDate = DateTime.UtcNow
                },
                new Warehouse {
                    WarehouseName = "Temporary Warehouse",
                    WarehouseCode = "Temp-123",
                    WarehouseAddress = "123 main st",
                    IsActive = true,
                    Capacity = 300,
                    CreatedDate = DateTime.UtcNow
                }
            };
            await context.Warehouses.AddRangeAsync(warehouses);
            await context.SaveChangesAsync();
        }
        
        if (!await context.ProductStocks.AnyAsync())
        {
            try
            {
                // Get warehouses by their unique codes
                var mainWarehouse = await context.Warehouses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(w => w.WarehouseCode == "Main-123");
                
                var secondaryWarehouse = await context.Warehouses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(w => w.WarehouseCode == "Second-1234");

                if (mainWarehouse == null || secondaryWarehouse == null)
                {
                    throw new Exception("Required warehouses not found in database");
                }

                // Get all products by SKU
                var productSkus = new[] { "ELEC-001", "OFFICE-100", "KIT-550", "CLO-2200", "ELEC-045" };
                var products = await context.Products
                    .AsNoTracking()
                    .Where(p => productSkus.Contains(p.SKU))
                    .ToListAsync();
                
                // Create stock entries
                var productStocks = new List<ProductStock>();

                foreach (var product in products)
                {
                    productStocks.Add(ProductStock.Create(productId: product.Id, mainWarehouse.Id, initialQuantity: 10));
                    productStocks.Add(ProductStock.Create(productId: product.Id, secondaryWarehouse.Id, initialQuantity: 500));
                }

                await context.ProductStocks.AddRangeAsync(productStocks);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error properly in production
                Console.WriteLine($"ERROR SEEDING PRODUCT STOCKS: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"INNER EXCEPTION: {ex.InnerException.Message}");
                }
            }
        }
    }
}