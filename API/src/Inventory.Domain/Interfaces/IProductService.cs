using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IProductService
{
    Task<HashSet<Product>> GetProductsByIds(HashSet<int> productIds);
    Task<HashSet<ProductStock>> GetProductsStocksInWarehouses(HashSet<int> productIds, HashSet<int> warehouseIds);
}