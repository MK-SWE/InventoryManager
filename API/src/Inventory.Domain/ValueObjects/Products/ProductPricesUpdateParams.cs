namespace Inventory.Domain.ValueObjects.Products;

public readonly struct ProductPricesUpdateParams( decimal? unitCost, decimal? unitPrice)
{
    public decimal? UnitCost { get; } = unitCost;
    public decimal? UnitPrice { get; } = unitPrice;
}