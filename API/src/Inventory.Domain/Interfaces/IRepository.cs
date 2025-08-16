using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IRepository<T>: IWriteRepository<T>, IReadRepository<T> where T : BaseEntity
{ }
