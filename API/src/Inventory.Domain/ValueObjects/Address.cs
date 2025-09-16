namespace Inventory.Domain.ValueObjects;

public sealed record Address
{
    public required string Line1 { get ; set; }
    public string? Line2 { get ; set; }
    public required string City { get ; set; }
    public string? State { get ; set; }
    public string? PostalCode { get ; set; }
    public required string Country { get ; set; }
    public static Address Create(string line1, string city, string country, string? line2, string? state, string? postalCode)
    {
        return new Address
        {
            Line1 = line1,
            Line2 = line2,
            City = city,
            State = state,
            PostalCode = postalCode,
            Country = country,
        };
    }
}