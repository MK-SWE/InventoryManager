using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Products;
using MediatR;

namespace Inventory.Application.Products.Queries;

public sealed record GetProductQuery(int Id) : IRequest<GetProductsResponseDto?>;
