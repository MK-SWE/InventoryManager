using Inventory.Application.Common.Interfaces;
using MediatR;

namespace Inventory.Application.StockReservation.Command;

public sealed record CancelStockReservationCommand(Guid ReservationReference) : IRequest<IReservationResult>
{ }