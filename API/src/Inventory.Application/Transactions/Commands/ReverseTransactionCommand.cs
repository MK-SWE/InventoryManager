using MediatR;

namespace Inventory.Application.Transactions.Commands;

public sealed record ReverseTransactionCommand(): IRequest;