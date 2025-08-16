using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Warehouses.Queries;

public sealed record GetWarehouseQuery(int Id) : IRequest<Warehouse>;