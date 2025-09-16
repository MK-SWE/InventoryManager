using Inventory.Domain.ValueObjects;

namespace Inventory.Application.Warehouses.DTOs;

public sealed record UpdateWarehouseDto
{
    public string? WarehouseCode { get; set; } = null;
    public string? WarehouseName { get; set; } = null;
    public Address? WarehouseAddress { get; set; } = null;
    public bool? IsActive { get; set; } = null;
    public int? Capacity { get; set; } = null;
};
