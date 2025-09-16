using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class InventoryTransaction : BaseEntity
{
    public TransactionType TransactionType { get; private set; }
    public required string ReferenceNumber { get; set; }
    public int? SourceWarehouseId { get; private set; } // Nullable for external sources
    public Warehouse? SourceWarehouse { get; private set; }
    public int? DestinationWarehouseId { get; private set; } // Nullable for external destinations
    public Warehouse? DestinationWarehouse { get; private set; }
    // public int? SupplierId { get; private set; } // Required for purchases, null otherwise
    // public Supplier? Supplier { get; private set; }
    // public int? CustomerId { get; private set; } // Required for sales, null otherwise
    // public Customer? Customer { get; private set; }
    public string? Notes { get; private set; }
    
    private readonly List<InventoryTransactionLine> _lines = new();
    public IReadOnlyList<InventoryTransactionLine> Lines => _lines.AsReadOnly();

    // Factory method to enforce business rules
    public static InventoryTransaction Create(
        TransactionType type,
        string reference,
        int? destinationWarehouseId,
        int? sourceWarehouseId,
        // int? supplierId,
        // int? customerId,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Reference number is required");

        // Validate transaction type rules
        switch (type)
        {
            case TransactionType.PurchaseReceipt:
                if (!destinationWarehouseId.HasValue /*|| !supplierId.HasValue*/)
                    throw new ArgumentException("Purchase requires destination warehouse and supplier.");
                return new InventoryTransaction
                {
                    TransactionType = type,
                    ReferenceNumber = reference,
                    SourceWarehouseId = null,
                    DestinationWarehouseId = destinationWarehouseId,
                    // SupplierId = supplierId,
                    // CustomerId = null,
                    Notes = notes
                };
            
            case TransactionType.SalesFulfillment:
                if (!sourceWarehouseId.HasValue /*|| !customerId.HasValue*/)
                    throw new ArgumentException("Sale requires source warehouse and customer.");
                return new InventoryTransaction
                {
                    TransactionType = type,
                    ReferenceNumber = reference,
                    SourceWarehouseId = sourceWarehouseId,
                    DestinationWarehouseId = null,
                    // SupplierId = null,
                    // CustomerId = customerId,
                    Notes = notes
                };
            
            case TransactionType.StockTransfer:
                if (!sourceWarehouseId.HasValue || !destinationWarehouseId.HasValue)
                    throw new ArgumentException("Transfer requires source and destination warehouses.");
                return new InventoryTransaction
                {
                    TransactionType = type,
                    ReferenceNumber = reference,
                    SourceWarehouseId = sourceWarehouseId,
                    DestinationWarehouseId = destinationWarehouseId,
                    // SupplierId = null,
                    // CustomerId = null,
                    Notes = notes
                };
            
            case TransactionType.StockAdjustment:
                if (!destinationWarehouseId.HasValue)
                    throw new ArgumentException("Stock adjustment require destination warehouse");
                return new InventoryTransaction
                {
                    TransactionType = type,
                    ReferenceNumber = reference,
                    SourceWarehouseId = null,
                    DestinationWarehouseId = destinationWarehouseId,
                    // SupplierId = null,
                    // CustomerId = null,
                    Notes = notes
                };
            
            case TransactionType.Return:
                if (!destinationWarehouseId.HasValue /*|| !customerId.HasValue*/)
                    throw new ArgumentException("Stock return require destination warehouse and customer Id");
                return new InventoryTransaction
                {
                    TransactionType = type,
                    ReferenceNumber = reference,
                    SourceWarehouseId = null,
                    DestinationWarehouseId = destinationWarehouseId,
                    // SupplierId = null,
                    // CustomerId = null,
                    Notes = notes
                };
            
            case TransactionType.ProductionInput:
                if (!sourceWarehouseId.HasValue)
                    throw new ArgumentException("Production input require source warehouse");
                return new InventoryTransaction
                {
                    TransactionType = type,
                    ReferenceNumber = reference,
                    SourceWarehouseId = sourceWarehouseId,
                    DestinationWarehouseId = null,
                    // SupplierId = null,
                    // CustomerId = null,
                    Notes = notes
                };
            
            case TransactionType.ProductionOutput:
                if (!sourceWarehouseId.HasValue)
                    throw new ArgumentException("Production output require destination warehouse");
                return new InventoryTransaction
                {
                    TransactionType = type,
                    ReferenceNumber = reference,
                    SourceWarehouseId = null,
                    DestinationWarehouseId = destinationWarehouseId,
                    // SupplierId = null,
                    // CustomerId = null,
                    Notes = notes
                };
            
            default:
                throw new InvalidOperationException();
        }
    }

    public void AddLine(int productId, int quantity, decimal unitCost)
    {
        _lines.Add(InventoryTransactionLine.Create(productId, quantity, unitCost));
    }
}