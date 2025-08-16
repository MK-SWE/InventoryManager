using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == parentId && !c.IsDeleted)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryTreeAsync(int rootId)
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == rootId && !c.IsDeleted);
    }
}