using Inventory.Application.Common.Exceptions;
using Inventory.Application.Warehouses.Queries;
using Inventory.Domain.Interfaces;
using Inventory.Shared.DTOs.Warehouses;
using Inventory.Shared.ValueObjects;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class GetWarehouseQueryHandler: IRequestHandler<GetWarehouseQuery, GetWarehouseResponseDto?>
{
    private readonly IWarehouseRepository _warehousesRepository;

    public GetWarehouseQueryHandler(IWarehouseRepository warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }

    public async Task<GetWarehouseResponseDto?> Handle(GetWarehouseQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = await _warehousesRepository.GetByIdAsync(request.Id);
            if (warehouse != null)
            {
                return new GetWarehouseResponseDto
                {
                    Id = warehouse.Id,
                    WarehouseCode = warehouse.WarehouseCode,
                    WarehouseName = warehouse.WarehouseName,
                    Capacity = warehouse.Capacity,
                    Address = new AddressResponse
                    {
                        Line1 = warehouse.WarehouseAddress.Line1,
                        Line2 = warehouse.WarehouseAddress.Line2,
                        City = warehouse.WarehouseAddress.City,
                        State = warehouse.WarehouseAddress.State,
                        PostalCode = warehouse.WarehouseAddress.PostalCode,
                        Country = warehouse.WarehouseAddress.Country,
                    }
                };
            }

            throw new KeyNotFoundException();
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Warehouse {request.Id} not found", ex);
        }
    }
}