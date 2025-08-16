using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
    Task<Category?> GetCategoryTreeAsync(int rootId);
}