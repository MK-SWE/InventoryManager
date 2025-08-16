namespace Inventory.Domain.Entities;

public sealed class Category: BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    
    public ICollection<Product> Products { get; set; } = [];
    public ICollection<Category> SubCategories { get; set; } = [];
}