using AutoMapper;
using Inventory.Application.Warehouses.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class CreateWarehouseCommandHandler: IRequestHandler<CreateWarehouseCommand, int>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;

    public CreateWarehouseCommandHandler(
        IWarehouseRepository warehouseRepository,
        IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }
    
    public async Task<int> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        Warehouse warehouse = _mapper.Map<Warehouse>(request.CreateWarehouseDto);

        int warehouseId = await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        
        return warehouseId;
    }
}