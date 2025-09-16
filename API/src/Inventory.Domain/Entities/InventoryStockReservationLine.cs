namespace Inventory.Domain.Entities;

public class InventoryStockReservationLine: BaseEntity
{
    public int ReservationId { get; set; }
    public InventoryStockReservation Reservation { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int ReservedQuantity { get; set; }
    public int AllocatedQuantity { get; set; }
    
    public static InventoryStockReservationLine Create(int productId, int reservedQuantity, int? allocatedQuantity)
    {
        if (productId <= 0)
            throw new ArgumentException("Invalid product ID", nameof(productId));
        if (reservedQuantity < 0)
            throw new ArgumentException("Reserved quantity cannot be negative", nameof(reservedQuantity));
        return new InventoryStockReservationLine()
        {
            ProductId = productId,
            ReservedQuantity = reservedQuantity,
            AllocatedQuantity = allocatedQuantity ?? 0
        };
    }
    
    public void Update(int? reservedQuantity, int? allocatedQuantity)
    {
        if (reservedQuantity.HasValue)
        {
            if (reservedQuantity.Value < 0)
                throw new ArgumentException("Reserved quantity cannot be negative", nameof(reservedQuantity));
            ReservedQuantity = reservedQuantity.Value;
            LastModifiedDate = DateTime.UtcNow;
        }
        if (allocatedQuantity.HasValue)
        {
            if (allocatedQuantity.Value < 0)
                throw new ArgumentException("Allocated quantity cannot be negative", nameof(allocatedQuantity));
            AllocatedQuantity = allocatedQuantity.Value;
            LastModifiedDate = DateTime.UtcNow;
        }
    }
}