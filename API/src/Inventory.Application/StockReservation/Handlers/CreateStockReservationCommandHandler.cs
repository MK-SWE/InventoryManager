using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.Command;
using MediatR;

namespace Inventory.Application.StockReservation.Handlers;

public class CreateStockReservationCommandHandler: IRequestHandler<CreateStockReservationCommand, IReservationResult>
{
    private readonly IInventoryStockReservationService _inventoryStockReservationService;

    public CreateStockReservationCommandHandler(IInventoryStockReservationService inventoryStockReservationService)
    {
        _inventoryStockReservationService = inventoryStockReservationService;
    }
    
    public async Task<IReservationResult> Handle(CreateStockReservationCommand request, CancellationToken cancellationToken)
    {
        var response = await _inventoryStockReservationService.ReserveStockAsync(request.CreateReservationCommandDto.ReserveItems, cancellationToken);
        return response;
    }
}