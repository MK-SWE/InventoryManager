namespace Inventory.Application.Warehouses.DTOs;

public sealed record CreateWarehouseDto
{
    public required string WarehouseCode { get; set; }
    public required string WarehouseName { get; set; }
    public required string WarehouseAddress { get; set; }
    public bool IsActive { get; set; }
    public int Capacity { get; set; }
};