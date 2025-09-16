using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.Command;
using Inventory.Application.StockReservation.ValueObjects;
using MediatR;

namespace Inventory.Application.StockReservation.Handlers;

public class AllocateStockReservationCommandHandler: IRequestHandler<AllocateStockReservationCommand, ReservationOperationResult>
{
    private readonly IInventoryStockReservationService _inventoryStockReservationService;

    public AllocateStockReservationCommandHandler(IInventoryStockReservationService inventoryStockReservationService)
    {
        _inventoryStockReservationService = inventoryStockReservationService;
    }
    
    public async Task<ReservationOperationResult> Handle(AllocateStockReservationCommand request, CancellationToken cancellationToken)
    {
        return await _inventoryStockReservationService.AllocateReservationAsync(request.ReservationReference,
            request.StockReservationAllocationDto);
    }
}