namespace Inventory.Shared.DTOs.Warehouses;

public sealed record CreateWarehouseRequestDto
{
    public required string WarehouseCode { get; set; }
    public required string WarehouseName { get; set; }
    public required string WarehouseAddress { get; set; }
    public int Capacity { get; set; }
}