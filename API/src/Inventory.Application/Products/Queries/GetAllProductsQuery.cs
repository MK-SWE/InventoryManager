using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Products;
using MediatR;

namespace Inventory.Application.Products.Queries;

public sealed record GetAllProductsQuery(int pageNumber, int PageSize): IRequest<(IReadOnlyList<GetProductsResponseDto> Items, int TotalCount)>;