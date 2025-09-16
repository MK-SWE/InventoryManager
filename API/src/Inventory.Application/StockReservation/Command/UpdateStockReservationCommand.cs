using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.DTOs;
using MediatR;

namespace Inventory.Application.StockReservation.Command;

public sealed record UpdateStockReservationCommand(Guid ReservationReference, UpdateStockReservationCommandDto CommandDto): IRequest<IReservationResult>;