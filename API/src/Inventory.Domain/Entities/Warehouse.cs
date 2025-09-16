using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Entities;

public sealed class Warehouse: BaseEntity
{
    public required string WarehouseCode { get; set; }
    public required string WarehouseName { get; set; }
    public required Address WarehouseAddress { get; set; }
    public bool IsActive { get; set; }
    public int Capacity { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public static Warehouse Create(string warehouseCode, string warehouseName, int capacity, string line1, string city, string country, string? line2, string? state, string? postalCode)
    {
        return new Warehouse
        {
            WarehouseCode = warehouseCode,
            WarehouseName = warehouseName,
            WarehouseAddress = Address.Create( line1, city, country,  line2,  state,  postalCode),
            Capacity = capacity
        };
    }
};