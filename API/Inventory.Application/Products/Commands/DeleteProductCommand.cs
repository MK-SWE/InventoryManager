using MediatR;

namespace Inventory.Application.Products.Commands;

public sealed record DeleteProductCommand(int Id): IRequest;