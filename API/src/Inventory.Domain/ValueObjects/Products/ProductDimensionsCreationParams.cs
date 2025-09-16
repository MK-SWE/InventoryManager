namespace Inventory.Domain.ValueObjects.Products;

public readonly struct ProductDimensionsCreationParams( int weight, int volume)
{
    public int Weight { get; } = weight;
    public int Volume { get; } = volume;
}