namespace Inventory.Domain.ValueObjects;

public sealed record StockStatus
{
    public int AvailableStock {get; set; }
    public int? OnHoldStock {get; set; }
    public int? QuarantinedStock {get; set; } 
    public int? QualityControlStock {get; set; }
    public int? ReturnedStock {get; set; }
    public int? DamagedStock {get; set; }
}