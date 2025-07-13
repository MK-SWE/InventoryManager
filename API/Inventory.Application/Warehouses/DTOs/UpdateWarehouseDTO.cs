namespace Inventory.Application.Warehouses.DTOs;

public sealed record UpdateWarehouseDTO
{
    public int? Id { get; set; } = null;
    public string? WarehouseName { get; set; } = null;
    public string? WarehouseAddress { get; set; } = null;
};

