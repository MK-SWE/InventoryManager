using Inventory.Shared.DTOs.InventoryTransactions;
using MediatR;

namespace Inventory.Application.Transactions.Queries;

public sealed record GetTransactionByIdQuery(int Id): IRequest<GetTransactionResponseDto>;