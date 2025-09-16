using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Warehouses;
using MediatR;

namespace Inventory.Application.Warehouses.Queries;

public sealed record GetAllWarehousesQuery : IRequest<IReadOnlyList<GetWarehouseResponseDto>>;