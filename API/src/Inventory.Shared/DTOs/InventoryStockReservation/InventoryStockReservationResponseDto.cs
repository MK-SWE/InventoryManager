namespace Inventory.Shared.DTOs.InventoryStockReservation;

public record InventoryStockReservationResponseDto
{
    public required int Id { get; set; }
    public required Guid ReservationReference { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public required DateTime? ExpiresAt { get; set; }
    public bool IsDeleted { get; set; }
    public required List<InventoryStockReservationLineResponseDto> Lines { get; set; } = new();
};