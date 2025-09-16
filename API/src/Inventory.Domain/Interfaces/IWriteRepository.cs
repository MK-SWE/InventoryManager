using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IWriteRepository<T> where T : BaseEntity
{
    Task<int> AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> HardDeleteAsync(int id, CancellationToken ct = default);
}