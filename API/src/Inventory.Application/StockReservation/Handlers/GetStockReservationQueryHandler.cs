using Inventory.Application.StockReservation.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Shared.DTOs.InventoryStockReservation;
using MediatR;

namespace Inventory.Application.StockReservation.Handlers;

public class GetStockReservationQueryHandler: IRequestHandler<GetStockReservationQuery, InventoryStockReservationResponseDto?>
{
    private readonly IInventoryStockReservationRepository _reservationRepositoryRepository;

    public GetStockReservationQueryHandler(IInventoryStockReservationRepository reservationRepositoryRepository)
    {
        _reservationRepositoryRepository = reservationRepositoryRepository;
    }

    public async Task<InventoryStockReservationResponseDto?> Handle(GetStockReservationQuery request, CancellationToken cancellationToken)
    {
        return await _reservationRepositoryRepository.GetByReferenceWithDetailsAsync(request.StockReservationId, cancellationToken);
    }
}