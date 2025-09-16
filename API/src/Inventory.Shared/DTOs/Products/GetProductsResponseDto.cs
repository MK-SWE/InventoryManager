namespace Inventory.Shared.DTOs.Products;

public sealed record GetProductsResponseDto()
{
        public int Id { get; set; }
        public string? SKU { get; init; }
        public string? ProductName { get; init; }
        public string? ProductDescription { get; init; }
        public string? Category { get; init; }
        public string? UnitOfMeasure { get; init; }
        public decimal? UnitPrice { get; init; }
        public int TotalStock { get; init; }
        public int TotalReservedStock { get; init; }
        public int TotalAllocatedStock { get; init; }
        public int TotalAvailableStock { get; set; }
        public int? ReorderLevel { get; init; }
        public int? Weight { get; init; }
        public int? Volume { get; init; }
        public DateTime? CreatedDate { get; init; }
        public DateTime? LastModifiedDate { get; init; }
        public bool? IsActive { get; init; }
};