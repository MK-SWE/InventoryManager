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
}
