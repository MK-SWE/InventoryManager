using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Warehouses;

namespace Inventory.Domain.Interfaces;

public interface IWarehouseRepository: IRepository<Warehouse>
{
    Task<GetWarehouseWithStockResponseDto?> GetWarehouseStocks(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}