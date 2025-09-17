using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Products;
using Inventory.Shared.DTOs.ProductsStock;

namespace Inventory.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<GetProductsResponseDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<GetProductsResponseDto> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
    Task<ProductWithStocksResponseDto?> GetByIdWithStocksAsync(int id, CancellationToken cancellationToken = default);
    
    Task<HashSet<Product>> GetBulkProductsByIdsAsync(HashSet<int> productIds, CancellationToken cancellationToken = default);
    Task UpdateBulkAsync(IReadOnlyCollection<Product> products, CancellationToken cancellationToken = default);
}