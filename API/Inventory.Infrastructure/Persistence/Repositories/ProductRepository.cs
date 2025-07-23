using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await Set.AnyAsync(p => p.Id == id, ct);

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
        => await Set.AsNoTracking()
                    .Include(p => p.ProductStocks)
                    .FirstOrDefaultAsync(p => p.SKU == sku, ct);

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken ct = default)
    {
        var query = Set.AsNoTracking();
        var total = await query.CountAsync(ct);
        
        var items = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
                              
        return (items, total);
    }
    
    public async Task<ProductWithStocksResponseDto?> GetByIdWithStocksAsync(int id, CancellationToken ct = default)
    {
        return await Set
            .Where(p => p.Id == id)
            .Select(p => new ProductWithStocksResponseDto
            {
                SKU = p.SKU,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                UnitPrice = p.UnitPrice,
                IsActive = p.IsActive,
                Quantity = p.ProductStocks.Select(ps => new ProductStockResponseDto
                {
                    WarehouseId = ps.WarehouseId,
                    Quantity = ps.Quantity
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);
    }
}