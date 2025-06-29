using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Queries;

public record GetAllProductsQuery(): IRequest<List<Product>>;