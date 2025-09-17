using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IProductStockRepository: IReadRepository<ProductStock>, IWriteRepository<ProductStock>
{
    Task<ProductStock?> GetByProductAndWarehouseAsync(
        int productId, 
        int warehouseId,
        CancellationToken cancellationToken = default);
        
    Task<IReadOnlyList<ProductStock>> GetByProductAsync(
        int productId,
        CancellationToken cancellationToken = default);
    

    Task<IEnumerable<ProductStock>> GetByProductsIdsAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);
    
    Task AddRangeAsync(IEnumerable<ProductStock> entities, CancellationToken cancellationToken);
    Task UpdateRangeAsync(IEnumerable<ProductStock> entities, CancellationToken cancellationToken);
}