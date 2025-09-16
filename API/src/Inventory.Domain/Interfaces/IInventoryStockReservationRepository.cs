using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.InventoryStockReservation;

namespace Inventory.Domain.Interfaces;

public interface IInventoryStockReservationRepository: IRepository<InventoryStockReservation>
{
    Task<InventoryStockReservationResponseDto?> GetByReferenceWithDetailsAsync(Guid reservationReference, CancellationToken ct = default);
    Task<InventoryStockReservation?> GetReservationWithLinesAsync(Guid reservationReference, CancellationToken ct = default);
}