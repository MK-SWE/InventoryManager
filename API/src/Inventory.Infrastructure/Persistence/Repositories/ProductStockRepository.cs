using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductStockRepository(AppDbContext context) : BaseRepository<ProductStock>(context), IProductStockRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ProductStock?> GetByProductAndWarehouseAsync(
        int productId, 
        int warehouseId,
        CancellationToken cancellationToken = default)
        => await _context.ProductStocks
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => 
                ps.ProductId == productId && 
                ps.WarehouseId == warehouseId, cancellationToken);

    public async Task<IReadOnlyList<ProductStock>> GetByProductAsync(
        int productId,
        CancellationToken cancellationToken = default)
        => await _context.ProductStocks
            .AsNoTracking()
            .Where(ps => ps.ProductId == productId)
            .ToListAsync(cancellationToken);
    

    public async Task<IEnumerable<ProductStock>> GetByProductsIdsAsync( IEnumerable<int> productIds, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productIds);
    
        // Materialize distinct IDs to avoid multiple enumeration
        var distinctIds = productIds.Distinct().ToArray();
    
        if (distinctIds.Length == 0)
            return Enumerable.Empty<ProductStock>();

        const int maxParameters = 1000; // SQL Server parameter limit
        if (distinctIds.Length <= maxParameters)
        {
            return await _context.ProductStocks
                .AsNoTracking()
                .Where(ps => distinctIds.Contains(ps.ProductId))
                .ToListAsync(cancellationToken);
        }

        var results = new List<ProductStock>();
        for (int i = 0; i < distinctIds.Length; i += maxParameters)
        {
            cancellationToken.ThrowIfCancellationRequested();
        
            var chunk = distinctIds
                .Skip(i)
                .Take(maxParameters)
                .ToArray();

            var stocks = await _context.ProductStocks
                .AsNoTracking()
                .Where(ps => chunk.Contains(ps.ProductId))
                .ToListAsync(cancellationToken);

            results.AddRange(stocks);
        }

        return results;
    }
    
    public async Task AddRangeAsync(IEnumerable<ProductStock> entities, CancellationToken cancellationToken )
        => await _context.ProductStocks.AddRangeAsync(entities, cancellationToken);
    
    public Task UpdateRangeAsync(IEnumerable<ProductStock> entities, CancellationToken cancellationToken)
    {
        _context.ProductStocks.UpdateRange(entities);
        return Task.CompletedTask;
    }
}
