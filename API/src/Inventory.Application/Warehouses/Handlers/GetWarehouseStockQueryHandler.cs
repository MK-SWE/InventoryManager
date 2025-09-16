using Inventory.Application.Warehouses.Queries;
using Inventory.Domain.Interfaces;
using Inventory.Shared.DTOs.Warehouses;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class GetWarehouseStockQueryHandler: IRequestHandler<GetWarehouseStockQuery, GetWarehouseWithStockResponseDto?>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public GetWarehouseStockQueryHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }
    public async Task<GetWarehouseWithStockResponseDto?> Handle(GetWarehouseStockQuery request, CancellationToken cancellationToken)
    {
        return await _warehouseRepository.GetWarehouseStocks(request.Id, cancellationToken);
    }
}