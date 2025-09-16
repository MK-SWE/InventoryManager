using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Interfaces;

public interface IInventoryStockService
{
    Task ReceiveStock(InventoryTransaction transaction, CancellationToken ct);
    Task ShipStock(InventoryTransaction transaction, CancellationToken ct);
    Task StockTransfer(InventoryTransaction transaction, CancellationToken ct);
    Task AdjustStock(InventoryTransaction transaction, CancellationToken ct);
}