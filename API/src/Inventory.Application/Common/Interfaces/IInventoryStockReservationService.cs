using Inventory.Application.StockReservation.DTOs;
using Inventory.Application.StockReservation.ValueObjects;

namespace Inventory.Application.Common.Interfaces;

public interface IInventoryStockReservationService
{
    Task<IReservationResult> ReserveStockAsync(IReadOnlyCollection<CreateReservationLineCommandDto> reservationLines, CancellationToken cancellationToken = default);
    Task<ReservationOperationResult> AllocateReservationAsync(Guid reference, StockReservationAllocationDto stockReservationAllocationDto, CancellationToken cancellationToken = default);
    Task<ReservationOperationResult> UpdateReservationAsync(Guid reference, UpdateStockReservationCommandDto updateDto,  CancellationToken cancellationToken = default);
    Task<ReservationOperationResult> CancelReservationAsync(Guid reference, CancellationToken cancellationToken = default);
}