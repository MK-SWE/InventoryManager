namespace Inventory.Domain.Interfaces;

public interface IWriteRepository<T>
{
    Task<T> CreateNewAsync(Func<T> createAction);
    Task<T> UpdateByIdAsync(int id, Action<T> updateAction);
    Task DeleteByIdAsync(int id);
}