namespace Inventory.Domain.Entities;

public sealed class Warehouse: BaseEntity
{
    public required string WarehouseCode { get; set; }
    public required string WarehouseName { get; set; }
    public required string WarehouseAddress { get; set; }
    public bool IsActive { get; set; }
    public int Capacity { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
};