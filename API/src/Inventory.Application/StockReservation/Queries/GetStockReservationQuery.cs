using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.InventoryStockReservation;
using MediatR;

namespace Inventory.Application.StockReservation.Queries;

public sealed record GetStockReservationQuery(Guid StockReservationId) : IRequest<InventoryStockReservationResponseDto?>
{ }