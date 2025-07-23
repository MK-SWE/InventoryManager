using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace Inventory.Infrastructure.Persistence.Repositories;

public class BaseRepository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context = context;
    protected DbSet<T> Set => _context.Set<T>();
    public IQueryable<T> Queryable => Set.AsQueryable();
    
    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await Set.FindAsync([id], ct);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().ToListAsync(ct);

    public virtual async Task<int> AddAsync(T entity, CancellationToken ct = default)
    {
        await Set.AddAsync(entity, ct);
        await SaveChangesWithId(entity);
        return entity.Id;
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        Set.Update(entity);
        await _context.SaveChangesAsync(ct);
    }
    
    public virtual async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await Set.FindAsync([id], ct);
        if (entity == null) return false;
    
        entity.IsDeleted = true;
        entity.LastModifiedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return true;
    }

    private async Task<int> SaveChangesWithId(T entity)
    {
        await _context.SaveChangesAsync();
        return (int)entity.GetType().GetProperty("Id")!.GetValue(entity)!;
    }
}