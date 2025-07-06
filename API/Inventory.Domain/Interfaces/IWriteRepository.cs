namespace Inventory.Domain.Interfaces;

public interface IWriteRepository<T>
{
    Task<int> CreateNewAsync(T createDTO);
    Task<T> UpdateByIdAsync(int id, T updateDTO);
    Task<bool> DeleteByIdAsync(int id);
}