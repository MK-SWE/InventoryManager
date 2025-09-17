using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Interfaces;

public interface IInventoryStockService
{
    Task ReceiveStock(InventoryTransaction transaction, CancellationToken cancellationToken);
    Task ShipStock(InventoryTransaction transaction, CancellationToken cancellationToken);
    Task StockTransfer(InventoryTransaction transaction, CancellationToken cancellationToken);
    Task AdjustStock(InventoryTransaction transaction, CancellationToken cancellationToken);
}