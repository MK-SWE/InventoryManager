namespace Inventory.Domain.Enums;

public enum TransactionType
{
    PurchaseReceipt = 1,
    SalesFulfillment,
    StockAdjustment,
    StockTransfer,
    Return,
    ProductionInput,
    ProductionOutput
}