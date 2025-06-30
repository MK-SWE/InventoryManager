using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Commands;

public record CreateProductCommand(CreateProductDTO NewProductDTO): IRequest<Product>;