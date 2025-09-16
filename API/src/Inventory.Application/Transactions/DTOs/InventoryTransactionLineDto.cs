namespace Inventory.Application.Transactions.DTOs;

public sealed record InventoryTransactionLineDto(
    int ProductId,
    int Quantity,
    decimal UnitCost
);