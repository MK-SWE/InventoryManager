using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductStockRepository(AppDbContext context) : IProductStockRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ProductStock?> GetByProductAndWarehouseAsync(
        int productId, 
        int warehouseId,
        CancellationToken ct = default)
        => await _context.ProductStocks
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => 
                ps.ProductId == productId && 
                ps.WarehouseId == warehouseId, ct);

    public async Task<IReadOnlyList<ProductStock>> GetByProductAsync(
        int productId,
        CancellationToken ct = default)
        => await _context.ProductStocks
            .AsNoTracking()
            .Where(ps => ps.ProductId == productId)
            .ToListAsync(ct);

    public async Task AddAsync(ProductStock stock, CancellationToken ct = default)
        => await _context.ProductStocks.AddAsync(stock, ct);

    public Task UpdateAsync(ProductStock stock, CancellationToken ct = default)
        => Task.FromResult(_context.ProductStocks.Update(stock));

    public Task DeleteAsync(ProductStock stock, CancellationToken ct = default)
        => Task.FromResult(_context.ProductStocks.Remove(stock));

    public async Task<IEnumerable<ProductStock>> GetByProductsIdsAsync( IEnumerable<int> productIds, CancellationToken ct = default)
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
                .ToListAsync(ct);
        }

        var results = new List<ProductStock>();
        for (int i = 0; i < distinctIds.Length; i += maxParameters)
        {
            ct.ThrowIfCancellationRequested();
        
            var chunk = distinctIds
                .Skip(i)
                .Take(maxParameters)
                .ToArray();

            var stocks = await _context.ProductStocks
                .AsNoTracking()
                .Where(ps => chunk.Contains(ps.ProductId))
                .ToListAsync(ct);

            results.AddRange(stocks);
        }

        return results;
    }
    
    public async Task AddRangeAsync(IEnumerable<ProductStock> entities, CancellationToken ct = default)
        => await _context.ProductStocks.AddRangeAsync(entities, ct);
    
    public Task UpdateRangeAsync(IEnumerable<ProductStock> entities, CancellationToken ct = default)
    {
        _context.ProductStocks.UpdateRange(entities);
        return Task.CompletedTask;
    }
}
