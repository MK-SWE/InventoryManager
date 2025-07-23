using Inventory.Domain.Entities;
using Inventory.Shared.DTOs;

namespace Inventory.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default);
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken ct = default);
    Task<ProductWithStocksResponseDto?> GetByIdWithStocksAsync(int id, CancellationToken ct = default);
}