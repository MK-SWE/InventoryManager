namespace Inventory.Domain.ValueObjects.Products;

public readonly struct ProductIdentificationUpdateParams(
    string? sku,
    string? name,
    string? description,
    int? categoryId,
    int? uomId,
    int? reorderLevel)
{
    
    public string? SKU { get; } = sku;
    public string? ProductName { get; } = name;
    public string? ProductDescription { get; } = description;
    public int? CategoryId { get; } = categoryId;
    public int? UnitOfMeasureId { get; } = uomId;
    public int? ReorderLevel { get; } = reorderLevel;
}