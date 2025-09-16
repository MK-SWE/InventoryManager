namespace Inventory.Domain.Enums;

public enum StockStatus
{
    Available= 1,
    OnHold,
    Quarantined, 
    QualityControl,
    Returned,
    Damaged
}