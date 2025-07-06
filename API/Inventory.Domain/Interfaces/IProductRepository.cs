namespace Inventory.Domain.Interfaces;

public interface IProductRepository<T> : IReadRepository<T>, IWriteRepository<T>
{
    Task<bool> ExistsAsync(int id);
    Task<T?> GetBySkuAsync(string sku);
    Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}