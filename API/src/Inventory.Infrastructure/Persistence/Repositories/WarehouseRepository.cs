using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseRepository(AppDbContext context) : IWarehouseRepository
{
    private readonly AppDbContext _context = context;
    private DbSet<Warehouse> Set => _context.Set<Warehouse>();

    public async Task<Warehouse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await Set.FindAsync([id], ct);
    }

    public async Task<IReadOnlyList<Warehouse>> GetAllAsync(CancellationToken ct = default)
    {
        return await Set.AsNoTracking().ToListAsync(ct);
    }

    public async Task<int> AddAsync(Warehouse entity, CancellationToken ct = default)
    {
        await Set.AddAsync(entity, ct);
        return entity.Id;
    }

    public async Task UpdateAsync(Warehouse entity, CancellationToken ct = default)
    {
        // Efficient update - only updates changed properties
        var entry = _context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            var existing = await Set.FindAsync([entity.Id, ct], cancellationToken: ct);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                return;
            }
        }
        entry.State = EntityState.Modified;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await Set.FindAsync([id], ct);
        if (entity == null) return false;
        
        Set.Remove(entity);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        var entity = await Set.FindAsync([id], ct);
        return entity != null;
    }
}