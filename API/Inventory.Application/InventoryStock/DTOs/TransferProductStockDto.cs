namespace Inventory.Application.InventoryStock.DTOs;

public sealed record TransferProductStockDto(
    int SourceWarehouseId,
    int DestinationWarehouseId,
    int Quantity
);