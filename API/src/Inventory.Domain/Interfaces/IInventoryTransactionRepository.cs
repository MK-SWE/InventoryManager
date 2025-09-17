using Inventory.Shared.DTOs.InventoryTransactions;

namespace Inventory.Domain.Interfaces;

public interface IInventoryTransactionRepository
{
    Task<GetTransactionResponseDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}