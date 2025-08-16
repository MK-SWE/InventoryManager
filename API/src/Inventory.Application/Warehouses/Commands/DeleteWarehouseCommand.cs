using MediatR;

namespace Inventory.Application.Warehouses.Commands;

public sealed record DeleteWarehouseCommand(int Id) : IRequest;