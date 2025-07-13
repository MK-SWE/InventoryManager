using Inventory.Application.Common.Exceptions;
using Inventory.Application.Warehouses.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class GetOneWarehouseQueryHandler: IRequestHandler<GetOneWarehouseQuery, Warehouse?>
{
    private readonly IReadRepository<Warehouse> _warehousesRepository;

    public GetOneWarehouseQueryHandler(IReadRepository<Warehouse> warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }

    public async Task<Warehouse?> Handle(GetOneWarehouseQuery request, CancellationToken cancellationToken)
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