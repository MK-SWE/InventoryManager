using MediatR;

namespace Inventory.Application.InventoryStock.Queries;

public sealed record GetStockByWarehouseQuery(): IRequest;