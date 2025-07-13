using Inventory.Application.Warehouses.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class DeleteWarehouseCommandHandler: IRequestHandler<DeleteWarehouseCommand>
{
    private readonly IWriteRepository<Warehouse> _warehouseRepository;

    public DeleteWarehouseCommandHandler(IWriteRepository<Warehouse> warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }
    
    public async Task Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _warehouseRepository.DeleteByIdAsync(request.Id);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Warehouse {request.Id} not found", ex);
        }
    }
}