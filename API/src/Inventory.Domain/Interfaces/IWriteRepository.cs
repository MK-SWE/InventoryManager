using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IWriteRepository<T> where T : BaseEntity
{
    Task<int> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> HardDeleteAsync(int id, CancellationToken cancellationToken = default);
}