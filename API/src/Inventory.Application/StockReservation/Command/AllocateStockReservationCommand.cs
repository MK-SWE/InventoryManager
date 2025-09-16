using Inventory.Application.StockReservation.DTOs;
using Inventory.Application.StockReservation.ValueObjects;
using MediatR;

namespace Inventory.Application.StockReservation.Command;

public sealed record AllocateStockReservationCommand(Guid ReservationReference, StockReservationAllocationDto StockReservationAllocationDto): IRequest<ReservationOperationResult>
{ }