using Inventory.Application.Warehouses.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using MediatR;

namespace Inventory.Application.Warehouses.Handlers;

public class CreateWarehouseCommandHandler: IRequestHandler<CreateWarehouseCommand, int>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly AppDbContext _context;

    public CreateWarehouseCommandHandler(IWarehouseRepository warehouseRepository, AppDbContext context )
    {
        _warehouseRepository = warehouseRepository;
        _context = context;
    }
    
    public async Task<int> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CreateWarehouseDto;
        var address = request.CreateWarehouseDto.WarehouseAddress;
        Warehouse warehouse = Warehouse.Create(dto.WarehouseCode, 
            dto.WarehouseName, 
            dto.Capacity, 
            address.Line1, 
            address.City, 
            address.Country, 
            address.Line2, 
            address.State, 
            address.PostalCode);

        int warehouseId = await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        
        return warehouseId;
    }
}