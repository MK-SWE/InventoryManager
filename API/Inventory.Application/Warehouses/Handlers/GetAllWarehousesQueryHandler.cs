﻿using Inventory.Application.Warehouses.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class GetAllWarehousesQueryHandler: IRequestHandler<GetAllWarehousesQuery, IReadOnlyList<Warehouse>>
{
    private readonly IWarehouseRepository _warehousesRepository;

    public GetAllWarehousesQueryHandler(IWarehouseRepository warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }

    public async Task<IReadOnlyList<Warehouse>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<Warehouse> warehouses = await _warehousesRepository.GetAllAsync(cancellationToken);
        return warehouses;
    }
}