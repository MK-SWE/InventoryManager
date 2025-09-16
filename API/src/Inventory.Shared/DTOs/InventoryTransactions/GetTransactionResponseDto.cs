using Inventory.Shared.DTOs.Warehouses;

namespace Inventory.Shared.DTOs.InventoryTransactions;

public sealed record GetTransactionResponseDto
{
    public int Id { get; init; }
    public required string TransactionType { get; set; }
    public required string ReferenceNumber { get; set; }
    public GetWarehouseResponseDto? SourceWarehouse { get; set; }
    public GetWarehouseResponseDto? DestinationWarehouse { get; set; }
    public List<TransactionLineDto> Lines { get; set; } = new();
    public string? Notes { get; set; }
    
}