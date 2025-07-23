using AutoMapper;
using Inventory.Application.Warehouses.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class UpdateWarehouseCommandHandler: IRequestHandler<UpdateWarehouseCommand, Warehouse>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;

    public UpdateWarehouseCommandHandler(IWarehouseRepository warehouseRepository, IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }
    
    public async Task<Warehouse> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (warehouse == null) throw new NotFoundException(nameof(Product), request.Id);
            
            _mapper.Map(request.UpdateWarehouse, warehouse);
            await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);
            return warehouse;
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Warehouse {request.Id} not found", ex);
        }
    }
}