using MediatR;

namespace Inventory.Application.Commands;

public record DeleteProductCommand(int Id): IRequest;