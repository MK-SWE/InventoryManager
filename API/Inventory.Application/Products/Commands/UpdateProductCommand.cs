using Inventory.Application.Products.DTOs;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Products.Commands;

public sealed record UpdateProductCommand(int Id, UpdateProductDto UpdateProduct): IRequest<Product>;