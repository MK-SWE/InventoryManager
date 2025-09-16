namespace Inventory.Domain.Entities;

public class InventoryTransactionLine : BaseEntity
{
    public int TransactionHeaderId { get; private set; }
    public InventoryTransaction Header { get; private set; } = null!;
    public int ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public int Quantity { get; private set; }
    public decimal? UnitCost { get; private set; }
    
    public static InventoryTransactionLine Create( int productId, int quantity, decimal unitCost)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive");
    
        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative");
        
        return new InventoryTransactionLine
        {
            ProductId = productId,
            Quantity = quantity,
            UnitCost = unitCost
        };
    }
}