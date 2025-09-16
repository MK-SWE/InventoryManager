using Inventory.Domain.Enums;

namespace Inventory.Application.Transactions.DTOs;

public sealed record InventoryTransactionDto
{
    public required TransactionType TransactionType { get; init; }
    public required string ReferenceNumber { get; init; }
    public int? SourceWarehouseId { get; init; }  // Null for external sources
    public int? DestinationWarehouseId { get; init; } // Null for external destinations
    public int? SupplierId { get; init; }         // Required for purchases
    public int? CustomerId { get; init; }         // Required for sales
    public string? Notes { get; init; }
    public required List<InventoryTransactionLineDto> Lines { get; init; } = [];
}