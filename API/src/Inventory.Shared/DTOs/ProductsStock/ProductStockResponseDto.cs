namespace Inventory.Shared.DTOs.ProductsStock;

public sealed record ProductStockResponseDto
{
    public int WarehouseId { get; init; }
    public int Quantity { get; init; }
};