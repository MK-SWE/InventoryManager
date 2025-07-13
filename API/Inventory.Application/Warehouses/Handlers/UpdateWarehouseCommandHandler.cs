using AutoMapper;
using Inventory.Application.Warehouses.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class UpdateWarehouseCommandHandler: IRequestHandler<UpdateWarehouseCommand, Warehouse>
{
    private readonly IWriteRepository<Warehouse> _warehouseRepository;
    private readonly IMapper _mapper;

    public UpdateWarehouseCommandHandler(IWriteRepository<Warehouse> warehouseRepository, IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }
    
    public async Task<Warehouse> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            request.UpdateWarehouse.Id = request.Id;
            var warehouse = _mapper.Map<Warehouse>(request.UpdateWarehouse);
            return await _warehouseRepository.UpdateByIdAsync(request.Id, warehouse);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Warehouse {request.Id} not found", ex);
        }
    }
}