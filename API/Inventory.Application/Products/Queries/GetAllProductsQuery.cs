using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Products.Queries;

public record GetAllProductsQuery(): IRequest<IReadOnlyList<Product>>;