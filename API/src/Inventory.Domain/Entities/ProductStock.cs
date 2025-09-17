using Inventory.Domain.Exceptions;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Entities;

public class ProductStock : BaseEntity
{
    public int ProductId { get; init; }
    public Product Product { get; init; } = null!;
    public int WarehouseId { get; init; }
    public Warehouse Warehouse { get; init; } = null!;
    public int Quantity { get; set; }
    public required StockStatus StockStatus { get; init; }

    public static ProductStock Create(int productId, int warehouseId, int quantity)
    {
        ProductStock productStock = new ProductStock
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            StockStatus = new StockStatus
            {
                AvailableStock = 0
            }
        };
        productStock.AddStock(quantity);
        return productStock;
    }
    
    private static void IsValidQuantity(int quantity) {
        if (quantity < 0)
            throw new InvalidStockOperationException("stock addition", "Quantity must be positive");
    }
    
    public void AddStock(int quantity)
    {
        IsValidQuantity(quantity);
        StockStatus.AvailableStock += quantity;
        Quantity += quantity;
    }

    public void AdjustStock(int quantity)
    {
        IsValidQuantity(quantity);
        Quantity = quantity;
    }
    
    public void ShipStock(int quantity)
    {
        IsValidQuantity(quantity);
        if (StockStatus.AvailableStock < quantity)
            throw new InvalidStockOperationException("stock shipment", "Insufficient available stock");
        StockStatus.AvailableStock -= quantity;
        Quantity -= quantity;
    }
    
    public void RemoveStock(int quantity)
    {
        IsValidQuantity(quantity);
        if (Quantity < quantity)
            throw new InvalidStockOperationException("stock removal", "Insufficient total stock");
        Quantity -= quantity;
        StockStatus.AvailableStock = Math.Max( StockStatus.AvailableStock - quantity, 0);
    }
    
    public void AllocateStock(int quantity)
    {
        IsValidQuantity(quantity);
        if (StockStatus.AvailableStock < quantity)
            throw new InvalidStockOperationException("stock allocation", "Insufficient available stock");
        StockStatus.AvailableStock -= quantity;
        StockStatus.OnHoldStock += quantity;
    }
}