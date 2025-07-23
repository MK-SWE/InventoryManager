using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public class ProductStock: BaseEntity
{
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int Quantity { get; private set; }

    public Product Product { get; init; } = null!;
    public Warehouse Warehouse { get; set; } = null!;

    public static ProductStock Create(int productId, int warehouseId, int initialQuantity)
        => new()
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = initialQuantity,
        };
    
    public void AddStock(int amount)
    {
        if (amount < 0) 
            throw new InvalidStockOperationException("stock addition", "Quantity must be positive");
        Quantity += amount;
    }

    public void RemoveStock(int amount)
    {
        if (amount < 0) 
            throw new InvalidStockOperationException("stock removal", "Quantity must be positive");
        if (Quantity < amount) 
            throw new InvalidStockOperationException("stock removal", "Insufficient quantity");
        Quantity -= amount;
    }

    public void AdjustStock(int amount)
    {
        if (amount < 0)
            throw new InvalidStockOperationException("stock adjustment", "Quantity cannot be negative");
        Quantity = amount;
    }
};