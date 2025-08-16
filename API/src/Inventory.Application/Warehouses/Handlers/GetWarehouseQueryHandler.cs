using Inventory.Application.Common.Exceptions;
using Inventory.Application.Warehouses.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class GetWarehouseQueryHandler: IRequestHandler<GetWarehouseQuery, Warehouse?>
{
    private readonly IWarehouseRepository _warehousesRepository;

    public GetWarehouseQueryHandler(IWarehouseRepository warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }

    public async Task<Warehouse?> Handle(GetWarehouseQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _warehousesRepository.GetByIdAsync(request.Id);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Warehouse {request.Id} not found", ex);
        }
    }
}