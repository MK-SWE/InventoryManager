using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Infrastructure.Persistence.Repositories.HelperMethods;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseRepository: IReadRepository<Warehouse>, IWriteRepository<Warehouse>
{
    private readonly AppDbContext _context;

    public WarehouseRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<Warehouse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Warehouses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        var warehouse = await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id);
        return warehouse;
    }

    public async Task<int> CreateNewAsync(Warehouse createWarehouseDTO)
    {
        await _context.Warehouses.AddAsync(createWarehouseDTO);
        await _context.SaveChangesAsync();
        return createWarehouseDTO.Id;
    }

    public async Task<Warehouse> UpdateByIdAsync(int id, Warehouse updateWarehouseDTO)
    {
        Warehouse? existing = await _context.Warehouses.FindAsync(id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Warehouse with ID {id} not found");
        }

        var properties = typeof(Warehouse).GetProperties()
            .Where(w => w.Name != "Id" && w.CanWrite);

        foreach (var property in properties)
        {
            var newValue = property.GetValue(updateWarehouseDTO);
            var defaultValue = GetDefaultValues.GetValue(property.PropertyType);
        
            // Update only if value is provided (not null or default)
            if (newValue != null && !newValue.Equals(defaultValue))
            {
                property.SetValue(existing, newValue);
            }
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        int rowsAffected = await _context.Warehouses
            .Where(w => w.Id == id)
            .ExecuteDeleteAsync();
    
        return rowsAffected > 0;
    }
}