namespace Inventory.Domain.Entities;

public record Warehouse
{
    public int Id { get; set; }
    public required string WarehouseName { get; set; }
    public required string WarehouseAddress { get; set; }
};