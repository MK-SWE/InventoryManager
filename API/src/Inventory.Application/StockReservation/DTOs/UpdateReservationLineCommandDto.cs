namespace Inventory.Application.StockReservation.DTOs;

public record UpdateReservationLineCommandDto(
    int ProductId, 
    int Quantity
);