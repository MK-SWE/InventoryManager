namespace Inventory.Application.StockReservation.DTOs;

public sealed record CreateReservationLineCommandDto(
    int ProductId, 
    int Quantity
    );