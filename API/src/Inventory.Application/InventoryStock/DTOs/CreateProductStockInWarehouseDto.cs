namespace Inventory.Application.InventoryStock.DTOs;

public sealed record CreateProductStockInWarehouseDto()
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Amount { get; set; }
};