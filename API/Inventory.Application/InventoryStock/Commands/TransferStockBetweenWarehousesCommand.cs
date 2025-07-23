using Inventory.Application.InventoryStock.DTOs;
using MediatR;

namespace Inventory.Application.InventoryStock.Commands;

public record TransferStockBetweenWarehousesCommand(int ProductId, TransferProductStockDto TransferProductStockDto): IRequest;
