namespace Inventory.Application.StockReservation.DTOs;

public record UpdateStockReservationCommandDto
{
    public DateTime? ExpiresAt { get; init; }
    public IReadOnlyCollection<UpdateReservationLineCommandDto>? ReservationLines { get; init; }
};