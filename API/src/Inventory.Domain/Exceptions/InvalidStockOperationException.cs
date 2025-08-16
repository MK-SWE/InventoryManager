namespace Inventory.Domain.Exceptions;

public sealed class InvalidStockOperationException : InventoryDomainException
{
    public InvalidStockOperationException(string operation, string reason) 
        : base($"Invalid {operation}: {reason}") { }
}