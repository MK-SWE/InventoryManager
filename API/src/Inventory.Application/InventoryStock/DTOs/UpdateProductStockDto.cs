using Inventory.Domain.Enums;

namespace Inventory.Application.InventoryStock.DTOs;

public sealed record UpdateProductStockDto(int NewQuantity, StockStatus StockStatus);