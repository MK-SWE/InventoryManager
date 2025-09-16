namespace Inventory.Shared.ValueObjects;

public sealed record AddressResponse
{
    public required string Line1 { get ; set; }
    public string? Line2 { get ; set; }
    public required string City { get ; set; }
    public string? State { get ; set; }
    public string? PostalCode { get ; set; }
    public required string Country { get ; set; }
}