using MediatR;

namespace Inventory.Application.InventoryStock.Commands;

public sealed record RemoveProductStockFromWarehouseCommand(int ProductId, int WarehouseId): IRequest;