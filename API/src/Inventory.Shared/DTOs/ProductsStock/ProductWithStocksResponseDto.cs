namespace Inventory.Shared.DTOs.ProductsStock;

public sealed record ProductWithStocksResponseDto
{
    public required int Id { get; set; }
    public required string SKU { get; init; }
    public required string ProductName { get; init; }
    public required string ProductDescription { get; init; }
    public required string Category { get; init; }
    public required string UnitOfMeasure { get; init; }
    public decimal UnitPrice { get; init; }
    public bool IsActive { get; init; }
    public IReadOnlyCollection<ProductStockResponseDto> Quantity { get; init; } = [];
};