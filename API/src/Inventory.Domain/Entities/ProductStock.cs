using System.Collections.ObjectModel;
using Inventory.Domain.Enums;
using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public class ProductStock: BaseEntity
{
    public int ProductId { get; init; }
    public int WarehouseId { get; init; }
    public int Quantity => _statusQuantities.Values.Sum();

    public Product Product { get; init; } = null!;
    public Warehouse Warehouse { get; set; } = null!;

    private readonly Dictionary<StockStatus, int> _statusQuantities;
    public IReadOnlyDictionary<StockStatus, int> StatusQuantities => 
        new ReadOnlyDictionary<StockStatus, int>(_statusQuantities);
    
    public ProductStock()
    {
        // Initialize all statuses with 0 quantity
        _statusQuantities = Enum.GetValues(typeof(StockStatus))
            .Cast<StockStatus>()
            .ToDictionary(status => status, _ => 0);
    }

    public static ProductStock Create(int productId, int warehouseId)
        => new()
        {
            ProductId = productId,
            WarehouseId = warehouseId,
        };
    
    public void AddStock(int amount, StockStatus status = StockStatus.Available)
    {
        if (amount < 0) 
            throw new InvalidStockOperationException("stock addition", "Quantity must be positive");
        _statusQuantities[status] += amount;
    }

    public void RemoveStock(int amount, StockStatus status)
    {
        if (amount < 0)
            throw new InvalidStockOperationException("stock removal", "Quantity must be positive");
    
        if (_statusQuantities[status] < amount)
            throw new InvalidStockOperationException("stock removal", $"Insufficient quantity in {status} status");
    
        _statusQuantities[status] -= amount;
    }

    // Move stock between statuses
    public void MoveStock(StockStatus fromStatus, StockStatus toStatus, int amount)
    {
        if (amount < 0)
            throw new InvalidStockOperationException("stock movement", "Quantity must be positive");
    
        if (_statusQuantities[fromStatus] < amount)
            throw new InvalidStockOperationException("stock movement", $"Insufficient quantity in {fromStatus} status");
    
        _statusQuantities[fromStatus] -= amount;
        _statusQuantities[toStatus] += amount;
    }
    
    public void AdjustStatusQuantity(StockStatus status, int newQuantity)
    {
        if (newQuantity < 0)
            throw new InvalidStockOperationException("status adjustment", "Quantity cannot be negative");
    
        _statusQuantities[status] = newQuantity;
    }
    
    public int GetQuantityByStatus(StockStatus status) => _statusQuantities[status];
};