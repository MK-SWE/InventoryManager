using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Products.Queries;

public record GetOneProductQuery(int Id) : IRequest<Product?>;
