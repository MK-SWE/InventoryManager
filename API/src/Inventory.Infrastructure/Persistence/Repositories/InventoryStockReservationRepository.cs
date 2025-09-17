using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Shared.DTOs.InventoryStockReservation;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryStockReservationRepositoryRepository: BaseRepository<InventoryStockReservation>, IInventoryStockReservationRepository
{
    private readonly AppDbContext _context;

    public InventoryStockReservationRepositoryRepository(AppDbContext context): base(context)
    {
        _context = context;
    }

    public Task<InventoryStockReservationResponseDto?> GetByReferenceWithDetailsAsync(Guid reservationReference, CancellationToken cancellationToken = default)
    {
        return _context.InventoryStockReservation
            .Where(res => res.ReservationReference == reservationReference)
            .Select(rs => new InventoryStockReservationResponseDto
            {
                Id = rs.Id,
                ReservationReference = rs.ReservationReference,
                CreatedAt = rs.CreatedDate,
                LastModifiedDate = rs.LastModifiedDate,
                ExpiresAt = rs.ExpiresAt,
                IsDeleted = rs.IsDeleted,
                Lines = rs.ReservationLines.Select(rl => new InventoryStockReservationLineResponseDto
                {
                    ProductName = rl.Product.ProductName,
                    Quantity = rl.ReservedQuantity
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<InventoryStockReservation?> GetReservationWithLinesAsync(Guid reservationReference, CancellationToken cancellationToken = default)
    {
        var reservation = await _context.InventoryStockReservation
            .Include(r => r.ReservationLines)
            .FirstOrDefaultAsync(r => r.ReservationReference == reservationReference, cancellationToken);

        if (reservation == null)
        {
            throw new KeyNotFoundException($"Reservation with reference {reservationReference} not found.");
        }

        return reservation;
    }
}