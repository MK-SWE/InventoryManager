using Inventory.Application.InventoryStock.DTOs;
using MediatR;

namespace Inventory.Application.InventoryStock.Commands;

public sealed record CreateProductStockInWarehouseCommand(CreateProductStockInWarehouseDto CreateProductStockInWarehouseDto): IRequest<int>;