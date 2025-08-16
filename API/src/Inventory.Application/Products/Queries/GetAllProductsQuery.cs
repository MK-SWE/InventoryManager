using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Products.Queries;

public sealed record GetAllProductsQuery(): IRequest<IReadOnlyList<Product>>;