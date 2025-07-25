﻿namespace Inventory.Domain.Entities;

public sealed class Product: BaseEntity
{ 
    public required string SKU { get; init; }
    public required string ProductName { get; init; }
    public required string ProductDescription { get; init; }
    public int CategoryId { get; init; }
    public int UnitOfMeasureId { get; init; }
    public decimal UnitPrice { get; init; }
    public int ReorderLevel { get; init; }
    public int Weight { get; init; }
    public int Volume { get; init; }
    public bool IsActive { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}