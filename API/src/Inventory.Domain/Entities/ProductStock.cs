using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public class ProductStock : BaseEntity
{
    public int ProductId { get; init; }
    public Product Product { get; init; } = null!;
    public int WarehouseId { get; init; }
    public Warehouse Warehouse { get; set; } = null!;
    public int Quantity { get; set; }
    
    // ToDo: Implement stock status management on a separate Database table and Domain entity,
    // this is just a placeholder for now
    public int AvailableStock { get; set; }
    public int OnHoldStock { get; set; }
    public int QuarantinedStock { get; set; }
    public int QualityControlStock { get; set; }
    public int ReturnedStock { get; set; }
    public int DamagedStock { get; set; }

    public static ProductStock Create(int productId, int warehouseId, int quantity)
    {
        ProductStock productStock = new ProductStock()
        {
            ProductId = productId,
            WarehouseId = warehouseId,
        };
        productStock.AddStock(quantity);
        return productStock;
    }

    public void AddStock(int quantity)
    {
        if (quantity < 0)
            throw new InvalidStockOperationException("stock addition", "Quantity must be positive");
        AvailableStock += quantity;
        Quantity += quantity;
    }

    public void AdjustStock(int quantity)
    {
        if (quantity < 0)
            throw new InvalidStockOperationException("stock adjustment", "Quantity must be positive");
        Quantity = quantity;
    }
    
    public void ShipStock(int quantity)
    {
        if (quantity < 0)
            throw new InvalidStockOperationException("stock shipment", "Quantity must be positive");
        if (AvailableStock < quantity)
            throw new InvalidStockOperationException("stock shipment", "Insufficient available stock");
        AvailableStock -= quantity;
        Quantity -= quantity;
    }
    
    public void RemoveStock(int quantity)
    {
        if (quantity < 0)
            throw new InvalidStockOperationException("stock removal", "Quantity must be positive");
        if (Quantity < quantity)
            throw new InvalidStockOperationException("stock removal", "Insufficient total stock");
        Quantity -= quantity;
        AvailableStock = Math.Max(AvailableStock - quantity, 0);
    }
    
    public void AllocateStock(int quantity)
    {
        if (quantity < 0)
            throw new InvalidStockOperationException("stock allocation", "Quantity must be positive");
        if (AvailableStock < quantity)
            throw new InvalidStockOperationException("stock allocation", "Insufficient available stock");
        AvailableStock -= quantity;
        OnHoldStock += quantity;
    }
}