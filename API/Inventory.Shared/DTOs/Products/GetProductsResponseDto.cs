namespace Inventory.Shared.DTOs.Products;

public record GetProductsResponseDto()
{
        public string? SKU { get; init; }
        public string? ProductName { get; init; }
        public string? ProductDescription { get; init; }
        public int? CategoryId { get; init; }
        public int? UnitOfMeasureId { get; init; }
        public decimal? UnitPrice { get; init; }
        public int? ReorderLevel { get; init; }
        public int? Weight { get; init; }
        public int? Volume { get; init; }
        public DateTime? CreatedDate { get; init; }
        public DateTime? LastModifiedDate { get; init; }
        public bool? IsActive { get; init; }
};