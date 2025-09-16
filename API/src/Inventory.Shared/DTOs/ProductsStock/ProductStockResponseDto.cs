namespace Inventory.Shared.DTOs.ProductsStock;

public sealed record ProductStockResponseDto
{
    public int WarehouseId { get; init; }
    public int TotalQuantity { get; init; }

    public ICollection<StockStatusResponseDto> StockStatus { get; init; } = null!;
};