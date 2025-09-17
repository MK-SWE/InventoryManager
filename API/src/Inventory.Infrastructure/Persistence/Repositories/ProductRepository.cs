using System.Data;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Shared.DTOs.Products;
using Inventory.Shared.DTOs.ProductsStock;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context): base(context)
    {
        _context = context;
    }
    
    public async Task<GetProductsResponseDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentTime = DateTime.UtcNow;
        var query = Set
            .AsNoTracking()
            .Include(p => p.ProductStocks)
            .Include(p => p.Reservations)
            .ThenInclude(r => r.Reservation); // Make sure to include the Reservation navigation property

        return await query
            .Where(product => product.Id == id)
            .Select(product => new GetProductsResponseDto
            {
                Id = product.Id,
                SKU = product.SKU,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Category = product.Category.Name,
                UnitOfMeasure = product.UnitOfMeasure.Name,
                UnitPrice = product.UnitPrice,
                TotalStock = product.ProductStocks.Sum(ps => ps.Quantity),
                TotalReservedStock = product.Reservations
                    .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > currentTime)
                    .Sum(line => line.ReservedQuantity),
                TotalAllocatedStock = product.Reservations
                    .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > currentTime)
                    .Sum(line => line.AllocatedQuantity),
                // Calculate TotalAvailableStock directly in the projection
                TotalAvailableStock = product.ProductStocks.Sum(ps => ps.Quantity) - 
                                      product.Reservations
                                          .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > currentTime)
                                          .Sum(line => line.ReservedQuantity),
                ReorderLevel = product.ReorderLevel,
                Weight = product.Weight,
                Volume = product.Volume,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => await Set.AnyAsync(p => p.Id == id, cancellationToken);

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        => await Set.AsNoTracking()
                    .Include(p => p.ProductStocks)
                    .FirstOrDefaultAsync(p => p.SKU == sku, cancellationToken);

    public async Task<(IReadOnlyList<GetProductsResponseDto> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var currentTime = DateTime.UtcNow;
        var query = Set
            .AsNoTracking()
            .Include(p => p.ProductStocks)
            .Include(p => p.Reservations)
            .ThenInclude(r => r.Reservation);

        var total = await query.CountAsync(cancellationToken);
    
        var items = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(product => new GetProductsResponseDto
            {
                Id = product.Id,
                SKU = product.SKU,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Category = product.Category.Name,
                UnitOfMeasure = product.UnitOfMeasure.Name,
                UnitPrice = product.UnitPrice,
                TotalStock = product.ProductStocks.Sum(ps => ps.Quantity),
                TotalReservedStock = product.Reservations
                    .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > currentTime)
                    .Sum(line => line.ReservedQuantity),
                TotalAllocatedStock = product.Reservations
                    .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > currentTime)
                    .Sum(line => line.AllocatedQuantity),
                TotalAvailableStock = product.ProductStocks.Sum(ps => ps.Quantity) - 
                                      product.Reservations
                                          .Where(line => !line.IsDeleted && line.Reservation.ExpiresAt > currentTime)
                                          .Sum(line => line.ReservedQuantity),
                ReorderLevel = product.ReorderLevel,
                Weight = product.Weight,
                Volume = product.Volume,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
            })
            .ToListAsync(cancellationToken);
                              
        return (items, total);
    }
    
    public async Task<ProductWithStocksResponseDto?> GetByIdWithStocksAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(p => p.Id == id)
            .Select(p => new ProductWithStocksResponseDto
            {
                Id = p.Id,
                SKU = p.SKU,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                Category = p.Category.Name,
                UnitOfMeasure = p.UnitOfMeasure.Code,
                UnitPrice = p.UnitPrice,
                IsActive = p.IsActive,
                Quantity = p.ProductStocks.Select(ps => new ProductStockResponseDto
                {
                    WarehouseId = ps.WarehouseId,
                    TotalQuantity = ps.Quantity
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<HashSet<Product>> GetBulkProductsByIdsAsync(HashSet<int> productIds, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productIds);
    
        // Materialize distinct IDs to avoid multiple enumeration
        var distinctIds = productIds.Distinct().ToArray();
    
        if (distinctIds.Length == 0)
            return new HashSet<Product>();
    
        const int maxParameters = 1000; // SQL Server parameter limit
        if (distinctIds.Length <= maxParameters)
        {
            var products = await Set
                .AsNoTracking()
                .Where(p => distinctIds.Contains(p.Id))
                .ToListAsync(cancellationToken);
            return products.ToHashSet();
        }
    
        var results = new HashSet<Product>();
        for (int i = 0; i < distinctIds.Length; i += maxParameters)
        {
            cancellationToken.ThrowIfCancellationRequested();
        
            var chunk = distinctIds.Skip(i).Take(maxParameters).ToArray();
            var products = await Set
                .AsNoTracking()
                .Where(p => chunk.Contains(p.Id))
                .ToListAsync(cancellationToken);
            results.UnionWith(products);
        }
        return results;
    }
    
    public async Task UpdateBulkAsync(IReadOnlyCollection<Product> products, CancellationToken cancellationToken = default)
    {
        if (products == null || !products.Any())
            throw new ArgumentException("Products collection cannot be null or empty.", nameof(products));
        
        var currentTime = DateTime.UtcNow;
        foreach (var product in products)
        {
            product.LastModifiedDate = currentTime;
        }
        
        try
        {
            Set.UpdateRange(products);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency issues for bulk updates
            foreach (var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);
                if (databaseValues == null)
                {
                    throw new DBConcurrencyException("One of the records was deleted by another user");
                }
                
                entry.OriginalValues.SetValues(databaseValues);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}