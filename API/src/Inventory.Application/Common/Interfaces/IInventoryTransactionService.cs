using Inventory.Application.Transactions.Commands;

namespace Inventory.Application.Common.Interfaces;

public interface IInventoryTransactionService
{
    Task<int> ProcessTransactionAsync(CreateTransactionCommand command, CancellationToken cancellationToken = default);
}