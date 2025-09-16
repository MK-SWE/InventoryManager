using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.DTOs;
using Inventory.Application.StockReservation.ValueObjects;
using MediatR;

namespace Inventory.Application.StockReservation.Command;

public sealed record CreateStockReservationCommand(CreateReservationCommandDto CreateReservationCommandDto): IRequest<IReservationResult>
{ } 