namespace Inventory.Domain.Interfaces;

public interface IReadRepository<T>
{
    Task<List<T>> GetAll();
    Task<T?> GetById(int id);
}