using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public sealed class UnitOfMeasure: BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public UomType Type { get; set; } = UomType.Quantity;
    public bool IsBaseUnit { get; set; } = false;
    public decimal? ConversionFactor { get; set; }
    
    public int? BaseUnitId { get; set; }
    public UnitOfMeasure? BaseUnit { get; set; }
    
    public ICollection<Product> Products { get; set; } = [];
}