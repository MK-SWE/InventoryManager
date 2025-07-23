using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.InventoryStock.Queries;

public sealed record GetStockByProductQuery(int ProductId) : IRequest<IReadOnlyList<ProductStock>>;