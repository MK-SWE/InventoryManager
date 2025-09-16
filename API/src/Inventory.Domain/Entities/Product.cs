using Inventory.Domain.ValueObjects.Products;

namespace Inventory.Domain.Entities;

public sealed class Product: BaseEntity
{
    #region IdentificationProperties
        public required string SKU { get; set; }
        public required string ProductName { get; set; }
        public required string ProductDescription { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
        public int ReorderLevel { get; set; }
        public bool IsActive { get; set; }
    #endregion
    
    #region DimensionsProperties
        public int Weight { get; set; }
        public int Volume { get; set; }
    #endregion
    
    #region PricesProperties
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
    
    #endregion
    
    #region DenormalizedStockFields
        public int TotalStock => ProductStocks.Sum(ps => ps.Quantity);

        public int TotalReservedStock =>
            Reservations
                .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > DateTime.UtcNow)
                .Where(line => line.ProductId == Id)
                .Sum(line => line.ReservedQuantity);
        
        public int TotalAllocatedStock =>
            Reservations
                .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > DateTime.UtcNow)
                .Where(line => line.ProductId == Id)
                .Sum(line => line.AllocatedQuantity);
        
        public int TotalAvailableStock => ProductStocks.Sum(ps => ps.AvailableStock);
        public int UnallocatedReservedStock => TotalReservedStock - TotalAllocatedStock;
        public bool CanReserve(int requestedQuantity) => requestedQuantity <= TotalAvailableStock;
    #endregion
    
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
    public ICollection<InventoryStockReservationLine> Reservations { get; set; } = new List<InventoryStockReservationLine>();
    
    public static Product Create(
        ProductIdentificationCreationParams identificationParams,
        ProductDimensionsCreationParams dimensionsParams,
        ProductPricesCreationParams pricesParams)
    {
        if (string.IsNullOrWhiteSpace(identificationParams.SKU))
            throw new ArgumentException("SKU is required");
    
        if (string.IsNullOrWhiteSpace(identificationParams.ProductName))
            throw new ArgumentException("Product name is required");
    
        if (string.IsNullOrWhiteSpace(identificationParams.ProductDescription))
            throw new ArgumentException("Description is required");
    
        if (pricesParams.UnitCost is < 0 or > int.MaxValue)
            throw new ArgumentException("Unit cost must be between 0 and 2,147,483,647");
    
        if (pricesParams.UnitPrice is < 0 or > int.MaxValue)
            throw new ArgumentException("Unit price cannot be negative");
    
        if (pricesParams.UnitPrice < pricesParams.UnitCost)
            throw new ArgumentException("Unit price cannot be less than unit cost");
    
        if (dimensionsParams.Weight is > 0 and > 1000)
            throw new ArgumentException("Weight must be positive");
    
        if (dimensionsParams.Volume is > 0 and > 100)
            throw new ArgumentException("Volume must be positive");
    
        if (identificationParams.ReorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative");
        return new Product
        {
            SKU = identificationParams.SKU,
            ProductName = identificationParams.ProductName,
            ProductDescription = identificationParams.ProductDescription,
            CategoryId = identificationParams.CategoryId,
            UnitOfMeasureId = identificationParams.UnitOfMeasureId,
            UnitCost = pricesParams.UnitCost,
            UnitPrice = pricesParams.UnitPrice,
            ReorderLevel = identificationParams.ReorderLevel,
            Weight = dimensionsParams.Weight,
            Volume = dimensionsParams.Volume,
            IsActive = true,
        };
    }
    
    public void Update(
        ProductIdentificationUpdateParams identificationParams,
        ProductDimensionsUpdateParams dimensionsParams,
        ProductPricesUpdateParams pricesParams)
    {
        if (identificationParams.SKU is not null)
            SKU = identificationParams.SKU;
        if (identificationParams.ProductName is not null)
            ProductName = identificationParams.ProductName;
        if (identificationParams.ProductDescription is not null)
            ProductDescription = identificationParams.ProductDescription;
        if (identificationParams.CategoryId.HasValue)
            CategoryId = identificationParams.CategoryId.Value;
        if (identificationParams.UnitOfMeasureId.HasValue)
            UnitOfMeasureId = identificationParams.UnitOfMeasureId.Value;
        if (pricesParams.UnitCost.HasValue)
            UnitCost = pricesParams.UnitCost.Value;
        if (pricesParams.UnitPrice.HasValue)
            UnitPrice = pricesParams.UnitPrice.Value;
        if (identificationParams.ReorderLevel.HasValue)
            ReorderLevel = identificationParams.ReorderLevel.Value;
        if (dimensionsParams.Weight.HasValue)
            Weight = dimensionsParams.Weight.Value;
        if (dimensionsParams.Volume.HasValue)
            Volume = dimensionsParams.Volume.Value;
    }
    
    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}