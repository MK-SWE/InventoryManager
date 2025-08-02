using System.ComponentModel.DataAnnotations;

namespace Inventory.Domain.ValueObjects;

public sealed record Address(
    [Required, StringLength(100)] string Line1,
    [StringLength(100)] string? Line2,
    [Required, StringLength(50)] string City,
    [Required, StringLength(50)] string State,
    [Required, StringLength(20)] string PostalCode,
    [Required, StringLength(50)] string Country
);