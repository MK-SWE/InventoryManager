namespace Inventory.Shared.DTOs.InventoryStockReservation;

public record InventoryStockReservationLineResponseDto
{
    public required string ProductName { get; init; } = null!;
    public required int Quantity { get; init;  }
};