namespace Inventory.Shared.DTOs.InventoryTransactions;

public sealed record InventoryTransactionLineRequestDto(
    int ProductId,
    int Quantity,
    decimal UnitCost
);