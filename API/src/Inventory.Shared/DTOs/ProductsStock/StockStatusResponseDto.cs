namespace Inventory.Shared.DTOs.ProductsStock;

public sealed record StockStatusResponseDto
{
    public string Status { get; init; } = null!; // This should match the StockStatus enum values
    public int Quantity { get; init; }
};