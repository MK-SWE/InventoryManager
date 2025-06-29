namespace Inventory.Domain.Interfaces;

public interface IWriteRepository<T>
{
    Task DeleteById(int id);
    Task UpdateById(int id, Action<T> updateAction);
}