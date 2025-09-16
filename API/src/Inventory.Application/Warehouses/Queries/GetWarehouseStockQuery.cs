using Inventory.Shared.DTOs.Warehouses;
using MediatR;

namespace Inventory.Application.Warehouses.Queries;

public sealed record GetWarehouseStockQuery(int Id): IRequest<GetWarehouseWithStockResponseDto?> { }