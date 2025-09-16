using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.Command;
using MediatR;

namespace Inventory.Application.StockReservation.Handlers;

public class UpdateStockReservationCommandHandler: IRequestHandler<UpdateStockReservationCommand, IReservationResult>
{
    private readonly IInventoryStockReservationService _reservationService;

    public UpdateStockReservationCommandHandler(IInventoryStockReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    public async Task<IReservationResult> Handle(UpdateStockReservationCommand request, CancellationToken cancellationToken)
    {
        return await _reservationService.UpdateReservationAsync(request.ReservationReference, request.CommandDto, cancellationToken);
    }
}