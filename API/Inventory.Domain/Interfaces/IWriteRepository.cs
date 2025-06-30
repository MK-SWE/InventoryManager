namespace Inventory.Domain.Interfaces;

public interface IWriteRepository<T>
{
    Task<T> CreateNew(Func<T> createAction);
    Task<T> UpdateById(int id, Action<T> updateAction);
    Task DeleteById(int id);
}