using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects;

namespace Inventory.Application.InventoryStock.DTOs;

public sealed record UpdateProductStockDto(int NewQuantity, StockStatus StockStatus);