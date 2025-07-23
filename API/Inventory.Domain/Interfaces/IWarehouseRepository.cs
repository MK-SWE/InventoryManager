using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IWarehouseRepository: IRepository<Warehouse>
{
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}