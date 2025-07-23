namespace Inventory.Shared.DTOs;

public sealed record ProductWithStocksResponseDto
{
    public required string SKU { get; init; }
    public required string ProductName { get; init; }
    public required string ProductDescription { get; init; }
    public decimal UnitPrice { get; init; }
    public bool IsActive { get; init; }
    public IReadOnlyCollection<ProductStockResponseDto> Quantity { get; init; } = [];
};