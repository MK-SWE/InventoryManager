using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository : IReadRepository<Product>, IWriteRepository<Product>
{
    private readonly List<Product> _repo;

    public ProductRepository()
    {
        // Initialize with dummy data
        _repo = new List<Product>
        {
            new Product {
                Id = 1,
                SKU = "ELEC-001",
                ProductName = "Wireless Mouse",
                ProductDescription = "Ergonomic wireless mouse with 2.4GHz connectivity",
                CategoryId = 1,
                UnitOfMeasureId = 1,
                UnitPrice = 19.99m,
                ReorderLevel = 50,
                Weight = 120,
                Volume = 150,
                IsActive = true
            },
            new Product {
                Id = 2,
                SKU = "OFFICE-100",
                ProductName = "Desk Lamp",
                ProductDescription = "LED desk lamp with adjustable brightness",
                CategoryId = 2,
                UnitOfMeasureId = 1,
                UnitPrice = 34.50m,
                ReorderLevel = 30,
                Weight = 850,
                Volume = 1200,
                IsActive = true
            },
            new Product {
                Id = 3,
                SKU = "KIT-550",
                ProductName = "Ceramic Coffee Mug",
                ProductDescription = "350ml ceramic mug with heat-resistant handle",
                CategoryId = 3,
                UnitOfMeasureId = 2,
                UnitPrice = 8.75m,
                ReorderLevel = 100,
                Weight = 350,
                Volume = 400,
                IsActive = true
            },
            new Product {
                Id = 4,
                SKU = "CLO-2200",
                ProductName = "Cotton T-Shirt",
                ProductDescription = "Premium cotton crew neck t-shirt",
                CategoryId = 4,
                UnitOfMeasureId = 3,
                UnitPrice = 24.99m,
                ReorderLevel = 75,
                Weight = 180,
                Volume = 300,
                IsActive = true
            },
            new Product {
                Id = 5,
                SKU = "ELEC-045",
                ProductName = "Bluetooth Earbuds",
                ProductDescription = "True wireless earbuds with charging case",
                CategoryId = 1,
                UnitOfMeasureId = 4,
                UnitPrice = 59.99m,
                ReorderLevel = 25,
                Weight = 50,
                Volume = 80,
                IsActive = false  // Inactive product
            }
        };
    }

    public Task<List<Product>> GetAll()
    {
        return Task.FromResult<List<Product>>(_repo);
    }

    public Task<Product?> GetById(int id)
    {
        var product = _repo.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> CreateNew(Func<Product> factory)
    {
        var product = factory();
    
        if (product.Id != 0)
        {
            throw new InvalidOperationException("New product must have Id=0");
        }

        // Auto-generate ID
        var newId = _repo.Count > 0 ? _repo.Max(p => p.Id) + 1 : 1;
        var createdProduct = product with { Id = newId };
    
        _repo.Add(createdProduct);
        return Task.FromResult(createdProduct);
    }
    
    public Task<Product> UpdateById(int id, Action<Product> updateAction)
    {
        var index = _repo.FindIndex(p => p.Id == id);
        if (index == -1) throw new KeyNotFoundException();
    
        var product = _repo[index];
        var updated = product with { };
        updateAction(updated);
    
        _repo[index] = updated;
        return Task.FromResult(_repo[index]);
    }
    
    public Task DeleteById(int id)
    {
        var productId = _repo.FindIndex(p => p.Id == id);
        if (productId == -1) throw new KeyNotFoundException();

        _repo.RemoveAt(productId);
        return Task.CompletedTask;
    }
}