using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IProductStockRepository: IReadRepository<ProductStock>, IWriteRepository<ProductStock>
{
    Task<ProductStock?> GetByProductAndWarehouseAsync(
        int productId, 
        int warehouseId,
        CancellationToken ct = default);
        
    Task<IReadOnlyList<ProductStock>> GetByProductAsync(
        int productId,
        CancellationToken ct = default);
    

    Task<IEnumerable<ProductStock>> GetByProductsIdsAsync(IEnumerable<int> productIds, CancellationToken ct = default);
    
    Task AddRangeAsync(IEnumerable<ProductStock> entities, CancellationToken ct);
    Task UpdateRangeAsync(IEnumerable<ProductStock> entities, CancellationToken ct);
}