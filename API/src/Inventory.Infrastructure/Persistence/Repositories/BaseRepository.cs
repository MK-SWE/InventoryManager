using System.Data;
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
    
    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await Set.FindAsync([id], cancellationToken);
    
    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Set.AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<int> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedDate = DateTime.UtcNow;
        entity.LastModifiedDate = DateTime.UtcNow;
        
        await Set.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.LastModifiedDate = DateTime.UtcNow;
        
        try
        {
            Set.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entry = ex.Entries.Single();
            var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);
            
            if (databaseValues == null)
            {
                throw new DBConcurrencyException("The record was deleted by another user");
            }
            
            entry.OriginalValues.SetValues(databaseValues);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    
    public virtual async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Set.FindAsync([id], cancellationToken);
        if (entity == null || entity.IsDeleted) 
            return false;
    
        entity.IsDeleted = true;
        entity.LastModifiedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    
    public virtual async Task<bool> HardDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Set.FindAsync([id], cancellationToken);
        if (entity == null) 
            return false;
        
        Set.Remove(entity);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}