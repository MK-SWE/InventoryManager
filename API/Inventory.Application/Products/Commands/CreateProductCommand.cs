using Inventory.Application.Products.DTOs;
using MediatR;

namespace Inventory.Application.Products.Commands;

public record CreateProductCommand(CreateProductDTO CreateProductDTO): IRequest<int>;