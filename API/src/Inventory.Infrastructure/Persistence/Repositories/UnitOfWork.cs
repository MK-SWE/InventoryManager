using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private IDbContextTransaction? _transaction;

    public IProductRepository Products => new ProductRepository(_context);
    public IWarehouseRepository Warehouses => new WarehouseRepository(_context);
    public IProductStockRepository ProductStocks => new ProductStockRepository(_context);

    public async Task CommitAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
        
        if (_transaction != null)
        {
            await _transaction.CommitAsync(ct);
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction == null) return;
        await _transaction.RollbackAsync(ct);
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaction = await _context.Database.BeginTransactionAsync(ct);

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}