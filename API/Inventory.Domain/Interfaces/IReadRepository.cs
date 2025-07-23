using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IReadRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
}