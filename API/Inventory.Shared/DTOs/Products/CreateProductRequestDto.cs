namespace Inventory.Shared.DTOs.Products;

public sealed record CreateProductRequestDto()
{
    public string? SKU { get; init; }
    public string? ProductName { get; init; }
    public string? ProductDescription { get; init; }
    public decimal UnitCost { get; init; }
    public decimal? UnitPrice { get; init; }
    public int? ReorderLevel { get; init; }
    public int Weight { get; init; }
    public int Volume { get; init; }
    public bool IsActive { get; init; } = true;
    
    public int? CategoryId { get; init; }
    public int UnitOfMeasureId { get; init; }
};