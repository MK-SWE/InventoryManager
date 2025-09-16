namespace Inventory.Domain.ValueObjects.Products;

public readonly struct ProductDimensionsUpdateParams( int? weight, int? volume)
{
    public int? Weight { get; } = weight;
    public int? Volume { get; } = volume;
}