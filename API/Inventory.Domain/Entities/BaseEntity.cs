using System.ComponentModel.DataAnnotations;

namespace Inventory.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedDate { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    public bool IsDeleted { get; set; } = false;
}