using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.Command;
using Inventory.Application.StockReservation.ValueObjects;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.StockReservation.Handlers;

public class CancelStockReservationCommandHandler: IRequestHandler<CancelStockReservationCommand, IReservationResult>
{
    private readonly IInventoryStockReservationService _inventoryStockReservationService;


    public CancelStockReservationCommandHandler(IInventoryStockReservationService inventoryStockReservationService)
    {
        _inventoryStockReservationService = inventoryStockReservationService;
    }
    
    public async Task<IReservationResult> Handle(CancelStockReservationCommand request, CancellationToken cancellationToken)
    {
        return await _inventoryStockReservationService.CancelReservationAsync(request.ReservationReference, cancellationToken);
    }
}