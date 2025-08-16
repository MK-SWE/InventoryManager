using System.ComponentModel.DataAnnotations;

namespace Inventory.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public byte[] RowVersion { get; set; } = [];

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}