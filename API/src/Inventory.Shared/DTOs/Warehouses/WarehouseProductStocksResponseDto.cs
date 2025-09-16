namespace Inventory.Shared.DTOs.Warehouses;

public sealed record WarehouseProductStocksResponseDto
{ 
    public required int Id { get; set; }
    public required string SKU { get; init; }
    public required string ProductName { get; init; }
    // public string? ProductDescription { get; init; }
    // public string? Category { get; init; }
    // public string? UnitOfMeasure { get; init; }
    // public decimal UnitPrice { get; init; }
    // public bool IsActive { get; init; }
    public required int Quantity { get; init; }
}