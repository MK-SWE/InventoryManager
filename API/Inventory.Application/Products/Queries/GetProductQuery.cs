using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Products.Queries;

public sealed record GetProductQuery(int Id) : IRequest<Product?>;
