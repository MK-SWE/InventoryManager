using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.Common.HelpingMethods;

public class ProductService: IProductService
{ 
    private readonly IProductRepository _productRepository;
    private readonly IProductStockRepository _productStockRepository;

    public ProductService(IProductRepository productRepository, IProductStockRepository productStockRepository)
    {
        _productRepository = productRepository;
        _productStockRepository = productStockRepository;
    }
    
    public async Task<HashSet<Product>> GetProductsByIds(HashSet<int> productIds)
    {
        var allProducts = await _productRepository.GetBulkProductsByIdsAsync(productIds);
        return allProducts;
    }

    public async Task<HashSet<ProductStock>> GetProductsStocksInWarehouses(HashSet<int> productIds,
        HashSet<int> warehouseIds)
    {
        var stocks = await _productStockRepository.GetByProductsIdsAsync(productIds);
        var filteredStocks = stocks.Where(ps => warehouseIds.Contains(ps.WarehouseId)).ToHashSet();
        return filteredStocks;
    }
}