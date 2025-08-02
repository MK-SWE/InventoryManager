using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Persistence.Seeders;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        // Seed Categories
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() {
                    Name = "Electronics",
                    Description = "Consumer electronics and digital accessories"
                },
                new() {
                    Name = "Office Furniture & Equipment",
                    Description = "Workspace solutions and office tools"
                },
                new() {
                    Name = "Kitchen & Dining",
                    Description = "Cooking appliances and culinary tools"
                },
                new() {
                    Name = "Clothing & Accessories",
                    Description = "Apparel and personal accessories"
                },
                new() {
                    Name = "Sports & Outdoors",
                    Description = "Fitness equipment and outdoor gear"
                },
                new() {
                    Name = "Beauty & Personal Care",
                    Description = "Skincare and beauty products"
                }, 
                new() {
                    Name = "Toys & Hobbies",
                    Description = "Recreational items and hobby kits"
                },
                new() {
                    Name = "Automotive",
                    Description = "Car accessories and maintenance products"
                },
                new() {
                    Name = "Health & Wellness",
                    Description = "Therapeutic and self-care devices"
                },
                new() {
                    Name = "Grocery",
                    Description = "Food items and pantry staples"
                }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} categories", categories.Count);
        }

        // Seed UnitOfMeasures
        if (!await context.UnitOfMeasures.AnyAsync())
        {
            var baseUnits = new List<UnitOfMeasure>
            {
                new() {
                    Code = "EA",
                    Name = "Each",
                    Type = UomType.Quantity,
                    IsBaseUnit = true
                },
                new() {
                    Code = "KG",
                    Name = "Kilogram",
                    Type = UomType.Weight,
                    IsBaseUnit = true
                },
                new() {
                    Code = "L",
                    Name = "Liter",
                    Type = UomType.Volume,
                    IsBaseUnit = true
                },
                new() {
                    Code = "M",
                    Name = "Meter",
                    Type = UomType.Length,
                    IsBaseUnit = true
                },
                new() {
                    Code = "M²",
                    Name = "Square Meter",
                    Type = UomType.Area,
                    IsBaseUnit = true
                }
            };

            await context.UnitOfMeasures.AddRangeAsync(baseUnits);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} base units", baseUnits.Count);

            var baseUnitEa = await context.UnitOfMeasures.FirstAsync(u => u.Code == "EA");
            var baseUnitKg = await context.UnitOfMeasures.FirstAsync(u => u.Code == "KG");
            var baseUnitL = await context.UnitOfMeasures.FirstAsync(u => u.Code == "L");
            var baseUnitM = await context.UnitOfMeasures.FirstAsync(u => u.Code == "M");

            var derivedUnits = new List<UnitOfMeasure>
            {
                new() {
                    Code = "PR",
                    Name = "Pair",
                    Type = UomType.Quantity,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitEa.Id,
                    ConversionFactor = 2  // 1 Pair = 2 Each
                },
                new() {
                    Code = "SET",
                    Name = "Set",
                    Type = UomType.Quantity,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitEa.Id,
                    ConversionFactor = 1  // 1 Set = 1 Each
                },
                new() {
                    Code = "DZ",
                    Name = "Dozen",
                    Type = UomType.Quantity,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitEa.Id,
                    ConversionFactor = 12  // 1 Dozen = 12 Each
                },
                new() {
                    Code = "G",
                    Name = "Gram",
                    Type = UomType.Weight,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitKg.Id,
                    ConversionFactor = 0.001m  // 1 Gram = 0.001 KG
                },
                new() {
                    Code = "LB",
                    Name = "Pound",
                    Type = UomType.Weight,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitKg.Id,
                    ConversionFactor = 0.453592m  // 1 LB = 0.453592 KG
                },
                new() {
                    Code = "OZ",
                    Name = "Ounce",
                    Type = UomType.Weight,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitKg.Id,
                    ConversionFactor = 0.0283495m  // 1 OZ = 0.0283495 KG
                },
                new() {
                    Code = "ML",
                    Name = "Milliliter",
                    Type = UomType.Volume,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitL.Id,
                    ConversionFactor = 0.001m  // 1 ML = 0.001 L
                },
                new() {
                    Code = "GAL",
                    Name = "Gallon",
                    Type = UomType.Volume,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitL.Id,
                    ConversionFactor = 3.78541m  // 1 GAL = 3.78541 L
                },
                new() {
                    Code = "CM",
                    Name = "Centimeter",
                    Type = UomType.Length,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitM.Id,
                    ConversionFactor = 0.01m  // 1 CM = 0.01 M
                },
                new() {
                    Code = "FT",
                    Name = "Foot",
                    Type = UomType.Length,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitM.Id,
                    ConversionFactor = 0.3048m  // 1 FT = 0.3048 M
                },
                new() {
                    Code = "YD",
                    Name = "Yard",
                    Type = UomType.Length,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitM.Id,
                    ConversionFactor = 0.9144m  // 1 YD = 0.9144 M
                },
                new() {
                    Code = "BOX",
                    Name = "Box",
                    Type = UomType.Packaging,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitEa.Id,
                    ConversionFactor = 1  // 1 Box = 1 Each
                },
                new() {
                    Code = "PK",
                    Name = "Pack",
                    Type = UomType.Packaging,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitEa.Id,
                    ConversionFactor = 6  // 1 Pack = 6 Each
                },
                new() {
                    Code = "RL",
                    Name = "Roll",
                    Type = UomType.Length,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitM.Id,
                    ConversionFactor = 1  // 1 Roll = 1 Meter
                },
                new() {
                    Code = "CTN",
                    Name = "Carton",
                    Type = UomType.Packaging,
                    IsBaseUnit = false,
                    BaseUnitId = baseUnitEa.Id,
                    ConversionFactor = 12  // 1 Carton = 12 Each
                }
            };
            
            await context.UnitOfMeasures.AddRangeAsync(derivedUnits);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} derived units", derivedUnits.Count);
        }
        
        // Seed Products
        if (!await context.Products.AnyAsync())
        {
            var electronics = await context.Categories.FirstAsync(c => c.Name == "Electronics");
            var office = await context.Categories.FirstAsync(c => c.Name == "Office Furniture & Equipment");
            var kitchen = await context.Categories.FirstAsync(c => c.Name == "Kitchen & Dining");
            var clothing = await context.Categories.FirstAsync(c => c.Name == "Clothing & Accessories");
            var sports = await context.Categories.FirstAsync(c => c.Name == "Sports & Outdoors");
            var beauty = await context.Categories.FirstAsync(c => c.Name == "Beauty & Personal Care");
            var toys = await context.Categories.FirstAsync(c => c.Name == "Toys & Hobbies");
            var auto = await context.Categories.FirstAsync(c => c.Name == "Automotive");
            var wellness = await context.Categories.FirstAsync(c => c.Name == "Health & Wellness");
            var grocery = await context.Categories.FirstAsync(c => c.Name == "Grocery");

            var uomEa = await context.UnitOfMeasures.FirstAsync(u => u.Code == "EA");
            var uomKg = await context.UnitOfMeasures.FirstAsync(u => u.Code == "KG");
            var uomM = await context.UnitOfMeasures.FirstAsync(u => u.Code == "M");
            var uomL = await context.UnitOfMeasures.FirstAsync(u => u.Code == "L");

            var products = new List<Product>
            {
                new() {
                    SKU = "CAM-15000",
                    ProductName = "4K Action Camera",
                    ProductDescription = "Waterproof adventure camera with image stabilization",
                    CategoryId = electronics.Id,
                    UnitOfMeasureId = uomEa.Id,
                    UnitPrice = 129.99m,
                    UnitCost = 102.69m,
                    ReorderLevel = 15,
                    Weight = 85,
                    Volume = 200,
                    IsActive = true
                },
                new() {
                    SKU = "FURN-15001",
                    ProductName = "Adjustable Standing Desk",
                    ProductDescription = "Electric height-adjustable desk with memory settings",
                    CategoryId = office.Id,
                    UnitOfMeasureId = uomEa.Id,
                    UnitPrice = 349.95m,
                    UnitCost = 279.96m,
                    ReorderLevel = 8,
                    Weight = 32000,
                    Volume = 180000,
                    IsActive = true
                },
                new() {
                    SKU = "COOK-15002",
                    ProductName = "Silicone Baking Mat Set",
                    ProductDescription = "Non-stick oven mats with measurement guides",
                    CategoryId = kitchen.Id,
                    UnitOfMeasureId = uomM.Id,
                    UnitPrice = 18.50m,
                    UnitCost = 13.32m,
                    ReorderLevel = 25,
                    Weight = 220,
                    Volume = 500,
                    IsActive = true
                },
                new() {
                    SKU = "OUTER-15003",
                    ProductName = "Lightweight Windbreaker",
                    ProductDescription = "Packable water-resistant jacket with hood",
                    CategoryId = clothing.Id,
                    UnitOfMeasureId = uomEa.Id,
                    UnitPrice = 45.75m,
                    UnitCost = 36.60m,
                    ReorderLevel = 30,
                    Weight = 280,
                    Volume = 1200,
                    IsActive = true
                },
                new() {
                    SKU = "GYM-15004",
                    ProductName = "Adjustable Dumbbell Set",
                    ProductDescription = "5-25kg quick-select dumbbells with stand",
                    CategoryId = sports.Id,
                    UnitOfMeasureId = uomEa.Id,
                    UnitPrice = 189.00m,
                    UnitCost = 151.20m,
                    ReorderLevel = 5,
                    Weight = 27500,
                    Volume = 45000,
                    IsActive = true
                },
                new() {
                    SKU = "SKIN-15005",
                    ProductName = "Hyaluronic Acid Serum",
                    ProductDescription = "Intense hydration formula with vitamin B5",
                    CategoryId = beauty.Id,
                    UnitOfMeasureId = uomM.Id,
                    UnitPrice = 28.95m,
                    UnitCost = 23.16m,
                    ReorderLevel = 40,
                    Weight = 45,
                    Volume = 100,
                    IsActive = true
                },
                new() {
                    SKU = "PUZ-15006",
                    ProductName = "3D Wooden Puzzle Set",
                    ProductDescription = "Architectural landmark models with laser-cut pieces",
                    CategoryId = toys.Id,
                    UnitOfMeasureId = uomEa.Id,
                    UnitPrice = 24.99m,
                    UnitCost = 18.74m,
                    ReorderLevel = 20,
                    Weight = 350,
                    Volume = 800,
                    IsActive = true
                },
                new() {
                    SKU = "AUTO-15007",
                    ProductName = "Car Jump Starter",
                    ProductDescription = "2000A lithium battery with USB ports and flashlight",
                    CategoryId = auto.Id,
                    UnitOfMeasureId = uomEa.Id,
                    UnitPrice = 89.99m,
                    UnitCost = 71.99m,
                    ReorderLevel = 12,
                    Weight = 850,
                    Volume = 1500,
                    IsActive = true
                },
                new() {
                    SKU = "WELL-15008",
                    ProductName = "Essential Oil Diffuser",
                    ProductDescription = "Ultrasonic aromatherapy device with color-changing lights",
                    CategoryId = wellness.Id,
                    UnitOfMeasureId = uomEa.Id,
                    UnitPrice = 32.25m,
                    UnitCost = 22.58m,
                    ReorderLevel = 25,
                    Weight = 380,
                    Volume = 900,
                    IsActive = true
                },
                new() {
                    SKU = "GRAN-15009",
                    ProductName = "Organic Quinoa",
                    ProductDescription = "1kg pack of pre-rinsed ancient grains",
                    CategoryId = grocery.Id,
                    UnitOfMeasureId = uomKg.Id,
                    UnitPrice = 9.99m,
                    UnitCost = 6.99m,
                    ReorderLevel = 60,
                    Weight = 1050,
                    Volume = 2500,
                    IsActive = true
                },
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} products", products.Count);
        }
        
        // Seed Warehouses
        if (!await context.Warehouses.AnyAsync())
        {
            var warehouses = new List<Warehouse>
            {
                new() {
                    WarehouseName = "Showroom Warehouse",
                    WarehouseCode = "Main-123",
                    WarehouseAddress = "123 main st",
                    IsActive = true,
                    Capacity = 500,  // Capacity in pallet locations
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new() {
                    WarehouseName = "Secondary Warehouse",
                    WarehouseCode = "Second-1234",
                    WarehouseAddress = "321 secondary st",
                    IsActive = true,
                    Capacity = 1000,  // Capacity in pallet locations
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new() {
                    WarehouseName = "Temporary Warehouse",
                    WarehouseCode = "Temp-123",
                    WarehouseAddress = "456 industrial ave",
                    IsActive = true,
                    Capacity = 300,  // Capacity in pallet locations
                    CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            };
            await context.Warehouses.AddRangeAsync(warehouses);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} warehouses", warehouses.Count);
        }
        
        // Seed Product Stocks
        if (!await context.ProductStocks.AnyAsync())
        {
            try
            {
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

                var products = await context.Products
                    .AsNoTracking()
                    .ToListAsync();
                
                if (!products.Any())
                {
                    logger.LogWarning("No products found for stock seeding");
                    return;
                }

                var productStocks = new List<ProductStock>();
                var rnd = new Random();

                foreach (var product in products)
                {
                    // Main warehouse: 10-50 units
                    productStocks.Add(ProductStock.Create( 
                        product.Id,
                        mainWarehouse.Id,
                        rnd.Next(10, 51)
                    ));
                    
                    // Secondary warehouse: 100-500 units
                    productStocks.Add(ProductStock.Create(
                        product.Id,
                        secondaryWarehouse.Id,
                        rnd.Next(100, 501)
                    ));
                }

                await context.ProductStocks.AddRangeAsync(productStocks);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded stock for {Count} products", products.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding product stocks");
                throw;
            }
        }
    }
}