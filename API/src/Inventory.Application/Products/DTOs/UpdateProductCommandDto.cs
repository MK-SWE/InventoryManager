namespace Inventory.Application.Products.DTOs;

public sealed record UpdateProductCommandDto
{
    public string? SKU { get; set; } = null;
    public string? ProductName { get; init; } = null;
    public string? ProductDescription { get; init; } = null;
    public int? CategoryId { get; init; } = null;
    public int? UnitOfMeasureId { get; init; } = null;
    public decimal? UnitPrice { get; init; } = null;
    public int? ReorderLevel { get; init; } = null;
    public int? Weight { get; init; } = null;
    public int? Volume { get; init; } = null;
    public bool? IsActive { get; init; } = null;
}