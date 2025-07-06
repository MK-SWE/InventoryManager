using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository : IReadRepository<Product>, IWriteRepository<Product>
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var product = await _context.Products
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<int> CreateNewAsync(Product createProductDTO)
    {
        await _context.Products.AddAsync(createProductDTO);
        await _context.SaveChangesAsync();
        return createProductDTO.Id;
    }
    
    public async Task<Product> UpdateByIdAsync(int id, Product updateProductDTO)
    {
        Product? existing = await _context.Products.FindAsync(id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found");
        }

        var properties = typeof(Product).GetProperties()
            .Where(p => p.Name != "Id" && p.CanWrite);

        foreach (var property in properties)
        {
            var newValue = property.GetValue(updateProductDTO);
            var defaultValue = GetDefaultValue(property.PropertyType);
        
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
        int rowsAffected = await _context.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    
        return rowsAffected > 0;
    }
    
    private static object? GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}