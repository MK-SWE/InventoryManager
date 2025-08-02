namespace Inventory.Domain.Entities;

public sealed class Product: BaseEntity
{ 
    public required string SKU { get; init; }
    public required string ProductName { get; init; }
    public required string ProductDescription { get; init; }
    public int CategoryId { get; init; }
    public Category Category { get; init; } = null!;
    public int UnitOfMeasureId { get; init; }
    public UnitOfMeasure UnitOfMeasure { get; init; } = null!;
    public decimal UnitCost { get; set; }
    public decimal UnitPrice { get; set; }
    public int ReorderLevel { get; init; }
    public int Weight { get; init; }
    public int Volume { get; init; }
    public bool IsActive { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}