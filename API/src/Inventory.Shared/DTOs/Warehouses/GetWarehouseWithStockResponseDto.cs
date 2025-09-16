using Inventory.Shared.DTOs.ProductsStock;
using Inventory.Shared.ValueObjects;

namespace Inventory.Shared.DTOs.Warehouses;

public record GetWarehouseWithStockResponseDto
{
    public int Id { get; set; }
    public required string WarehouseName { get; set; }
    public required string WarehouseCode { get; set; }
    public required int Capacity { get; set; }
    public required IReadOnlyList<WarehouseProductStocksResponseDto> ProductsStock { get; set; }
    public required AddressResponse Address { get; set; }
};