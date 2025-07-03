namespace Inventory.Domain.Interfaces;

public interface IReadRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
}