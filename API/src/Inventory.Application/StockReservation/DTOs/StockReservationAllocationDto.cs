namespace Inventory.Application.StockReservation.DTOs;

public sealed record StockReservationAllocationDto
{
    public required List<ProductAllocationDto> AllocateItems { get; init; }
}

public sealed record ProductAllocationDto
{
    public int ProductId { get; init; }
    public required List<WarehouseAllocationDto> WarehouseAllocations { get; init; }
}

public sealed record WarehouseAllocationDto
{
    public int WarehouseId { get; init; }
    public int AllocatedQuantity { get; init; }
}