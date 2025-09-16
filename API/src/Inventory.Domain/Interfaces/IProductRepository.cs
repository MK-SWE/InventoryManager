using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Products;
using Inventory.Shared.DTOs.ProductsStock;

namespace Inventory.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<GetProductsResponseDto?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default);
    Task<(IReadOnlyList<GetProductsResponseDto> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken ct = default);
    Task<ProductWithStocksResponseDto?> GetByIdWithStocksAsync(int id, CancellationToken ct = default);
    
    Task<HashSet<Product>> GetBulkProductsByIdsAsync(HashSet<int> productIds, CancellationToken ct = default);
    Task UpdateBulkAsync(IReadOnlyCollection<Product> products, CancellationToken ct = default);
}