using Inventory.Application.Products.DTOs;
using MediatR;

namespace Inventory.Application.Products.Commands;

public sealed record CreateProductCommand(CreateProductCommandDto CreateProductCommandDto): IRequest<int>;