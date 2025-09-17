using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Shared.DTOs.ProductsStock;
using Inventory.Shared.DTOs.Warehouses;
using Inventory.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseRepository(AppDbContext context) : BaseRepository<Warehouse>(context), IWarehouseRepository
{
    private readonly AppDbContext _context = context;

    public async Task<GetWarehouseWithStockResponseDto?> GetWarehouseStocks(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .Where(warehouse => warehouse.Id == id)
            .Select(warehouse => new GetWarehouseWithStockResponseDto
            {
                Id = warehouse.Id,
                WarehouseCode = warehouse.WarehouseCode,
                WarehouseName = warehouse.WarehouseName,
                Capacity = warehouse.Capacity,
                Address = new AddressResponse
                {
                    Line1 = warehouse.WarehouseAddress.Line1,
                    Line2 = warehouse.WarehouseAddress.Line2,
                    City = warehouse.WarehouseAddress.City,
                    State = warehouse.WarehouseAddress.State,
                    PostalCode = warehouse.WarehouseAddress.PostalCode,
                    Country = warehouse.WarehouseAddress.Country,
                },
                ProductsStock = warehouse.ProductStocks.Select(stock => new WarehouseProductStocksResponseDto
                {
                    Id = stock.Product.Id,
                    SKU = stock.Product.SKU,
                    ProductName = stock.Product.ProductName,
                    Quantity = stock.Quantity
                }).ToList(),
            }).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Set.FindAsync([id], cancellationToken);
        return entity != null;
    }
}