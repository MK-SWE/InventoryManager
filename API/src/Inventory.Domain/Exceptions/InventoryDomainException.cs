namespace Inventory.Domain.Exceptions;

public abstract class InventoryDomainException : Exception
{
    protected InventoryDomainException(string message) : base(message) { }
}