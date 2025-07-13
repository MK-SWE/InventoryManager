namespace Inventory.Application.Warehouses.DTOs;

public sealed record CreateWarehouseDTO
{
    public int Id { get; set; }
    public required string WarehouseName { get; set; }
    public required string WarehouseAddress { get; set; }
};