namespace Inventory.Domain.Interfaces;

public interface IReadRepository<T>
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(int id);
}