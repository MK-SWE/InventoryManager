namespace Inventory.Domain.Entities;

public record Product
{
    public int Id { get; set; }
    public required string SKU { get; set; }
    public required string ProductName { get; set; }
    public required string ProductDescription { get; set; }
    public int CategoryId { get; set; }
    public int UnitOfMeasureId { get; set; }
    public decimal UnitPrice { get; set; }
    public int ReorderLevel { get; set; }
    public int Weight { get; set; }
    public int Volume { get; set; }
    public bool IsActive { get; set; }
}