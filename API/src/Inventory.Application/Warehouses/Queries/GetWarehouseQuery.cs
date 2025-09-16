using Inventory.Shared.DTOs.Warehouses;
using MediatR;

namespace Inventory.Application.Warehouses.Queries;

public sealed record GetWarehouseQuery(int Id) : IRequest<GetWarehouseResponseDto>;