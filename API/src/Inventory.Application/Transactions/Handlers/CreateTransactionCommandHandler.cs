using Inventory.Application.Common.Interfaces;
using Inventory.Application.Transactions.Commands;

using MediatR;

namespace Inventory.Application.Transactions.Handlers;

public sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
{
    private readonly IInventoryTransactionService _transactionService;

    public CreateTransactionCommandHandler(
        IInventoryTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        return await _transactionService.ProcessTransactionAsync(request, cancellationToken);
    }
    
}