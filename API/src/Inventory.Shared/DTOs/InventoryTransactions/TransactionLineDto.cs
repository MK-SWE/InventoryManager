namespace Inventory.Shared.DTOs.InventoryTransactions;

public sealed record TransactionLineDto
{
    public int ProductId { get; set; }
    public required string SKU { get; set; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitCost { get; set; }
}