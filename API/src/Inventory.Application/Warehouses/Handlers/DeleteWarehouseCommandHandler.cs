using Inventory.Application.Warehouses.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class DeleteWarehouseCommandHandler: IRequestHandler<DeleteWarehouseCommand>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public DeleteWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }
    
    public async Task Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _warehouseRepository.ExistsAsync(request.Id, cancellationToken)) 
                throw new NotFoundException($"Warehouse with id: {request.Id}", "not found");
            await _warehouseRepository.DeleteAsync(request.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("an Error Occurred", ex);
        }
    }
}