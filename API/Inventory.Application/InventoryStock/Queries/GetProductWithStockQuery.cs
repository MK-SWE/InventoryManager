using Inventory.Shared.DTOs;
using MediatR;

namespace Inventory.Application.InventoryStock.Queries;

public sealed record GetProductWithStockQuery(int ProductId): IRequest<ProductWithStocksResponseDto>;