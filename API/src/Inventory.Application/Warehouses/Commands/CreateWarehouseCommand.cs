using Inventory.Application.Warehouses.DTOs;
using MediatR;

namespace Inventory.Application.Warehouses.Commands;

public sealed record CreateWarehouseCommand(CreateWarehouseDto CreateWarehouseDto) : IRequest<int>;