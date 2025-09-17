using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.InventoryStock.Commands;

public sealed record UpdateProductStockInWarehouseCommand(
    int ProductId, 
    int WarehouseId, 
    int Amount,
    StockStatus StockStatus) : IRequest;