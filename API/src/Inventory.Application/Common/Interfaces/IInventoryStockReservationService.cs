using Inventory.Application.StockReservation.DTOs;
using Inventory.Application.StockReservation.ValueObjects;

namespace Inventory.Application.Common.Interfaces;

public interface IInventoryStockReservationService
{
    Task<IReservationResult> ReserveStockAsync(IReadOnlyCollection<CreateReservationLineCommandDto> reservationLines, CancellationToken ct = default);
    Task<ReservationOperationResult> AllocateReservationAsync(Guid reference, StockReservationAllocationDto stockReservationAllocationDto, CancellationToken ct = default);
    Task<ReservationOperationResult> UpdateReservationAsync(Guid reference, UpdateStockReservationCommandDto updateDto,  CancellationToken ct = default);
    Task<ReservationOperationResult> CancelReservationAsync(Guid reference, CancellationToken ct = default);
}