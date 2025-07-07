using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Queries;

public record GetOneProductQuery(int Id) : IRequest<Product?>;
