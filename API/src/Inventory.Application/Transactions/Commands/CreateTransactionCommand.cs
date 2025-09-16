using Inventory.Application.Transactions.DTOs;
using MediatR;

namespace Inventory.Application.Transactions.Commands;

public sealed record CreateTransactionCommand(InventoryTransactionDto InventoryTransactionDto): IRequest<int> { }
