using Inventory.Application.Warehouses.DTOs;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Warehouses.Commands;

public sealed record UpdateWarehouseCommand(int Id, UpdateWarehouseDto UpdateWarehouse): IRequest<Warehouse>;