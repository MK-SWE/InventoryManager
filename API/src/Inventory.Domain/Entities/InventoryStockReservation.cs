namespace Inventory.Domain.Entities;

public class InventoryStockReservation : BaseEntity
{
    public required Guid ReservationReference { get; set; }
    private readonly List<InventoryStockReservationLine> _lines = new ();
    public ICollection<InventoryStockReservationLine> ReservationLines  => _lines; 
    public DateTime? ExpiresAt { get; set; }
    
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt <= DateTime.UtcNow;

    public static InventoryStockReservation Create(
        DateTime expiresAt)
    {
        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future", nameof(expiresAt));
            
        return new InventoryStockReservation()
        {
            ReservationReference = Guid.NewGuid(),
            ExpiresAt = expiresAt
        };
    }
    
    public void AddReservationLine(int productId, int reservedQuantity, int? allocatedQuantity = null)
    {
        _lines.Add(InventoryStockReservationLine.Create(productId, reservedQuantity, allocatedQuantity));
    }
}
