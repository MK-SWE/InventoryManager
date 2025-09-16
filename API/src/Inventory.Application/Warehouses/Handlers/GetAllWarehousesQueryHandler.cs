using Inventory.Application.Warehouses.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Shared.DTOs.Warehouses;
using Inventory.Shared.ValueObjects;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class GetAllWarehousesQueryHandler: IRequestHandler<GetAllWarehousesQuery, IReadOnlyList<GetWarehouseResponseDto>>
{
    private readonly IWarehouseRepository _warehousesRepository;

    public GetAllWarehousesQueryHandler(IWarehouseRepository warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }

    public async Task<IReadOnlyList<GetWarehouseResponseDto>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<Warehouse> warehouses = await _warehousesRepository.GetAllAsync(cancellationToken);
        IList<GetWarehouseResponseDto> response = new GetWarehouseResponseDto[warehouses.Count];
        for (int i = 0; i < warehouses.Count; i++)
        {
            var temp = new GetWarehouseResponseDto
            {
                Id = warehouses[i].Id,
                WarehouseCode = warehouses[i].WarehouseCode,
                WarehouseName = warehouses[i].WarehouseName,
                Capacity = warehouses[i].Capacity,
                Address = new AddressResponse
                {
                    Line1 = warehouses[i].WarehouseAddress.Line1,
                    Line2 = warehouses[i].WarehouseAddress.Line2,
                    City = warehouses[i].WarehouseAddress.City,
                    State = warehouses[i].WarehouseAddress.State,
                    PostalCode = warehouses[i].WarehouseAddress.PostalCode,
                    Country = warehouses[i].WarehouseAddress.Country,
                }
            };
            response[i] = temp;
        }
        return response.AsReadOnly();
    }
}