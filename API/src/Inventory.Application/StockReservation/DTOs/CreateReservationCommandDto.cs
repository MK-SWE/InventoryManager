namespace Inventory.Application.StockReservation.DTOs;

public sealed record CreateReservationCommandDto
{
    public DateTime ExpiresAt { get; init; }
    public required IReadOnlyCollection<CreateReservationLineCommandDto> ReserveItems { get; init; }
};