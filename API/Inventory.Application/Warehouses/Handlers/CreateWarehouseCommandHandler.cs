using AutoMapper;
using Inventory.Application.Warehouses.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class CreateWarehouseCommandHandler: IRequestHandler<CreateWarehouseCommand, int>
{
    private readonly IWriteRepository<Warehouse> _warehouseRepository;
    private readonly IMapper _mapper;

    public CreateWarehouseCommandHandler(
        IWriteRepository<Warehouse> warehouseRepository,
        IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }
    
    public async Task<int> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = _mapper.Map<Warehouse>(request.CreateWarehouseDTO);

        var warehouseId = await _warehouseRepository.CreateNewAsync(warehouse);
        
        return warehouseId;
    }
}