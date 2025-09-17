using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Domain.ValueObjects;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Shared.DTOs.InventoryTransactions;
using Inventory.Shared.DTOs.Warehouses;
using Inventory.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryTransactionsRepository: BaseRepository<InventoryTransaction>, IInventoryTransactionRepository
{
    private readonly AppDbContext _context;

    public InventoryTransactionsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<GetTransactionResponseDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.InventoryTransactionHeaders
            .Where(transaction => transaction.Id == id)
            .Select(transaction => new GetTransactionResponseDto
            {
                Id = transaction.Id,
                TransactionType = transaction.TransactionType.ToString(),
                ReferenceNumber = transaction.ReferenceNumber,
                SourceWarehouse = transaction.SourceWarehouse != null ? new GetWarehouseResponseDto
                {
                    Id = transaction.SourceWarehouse.Id,
                    WarehouseName = transaction.SourceWarehouse.WarehouseName,
                    WarehouseCode = transaction.SourceWarehouse.WarehouseCode,
                    Capacity = transaction.SourceWarehouse.Capacity,
                    Address = new AddressResponse
                    {
                        Line1 = transaction.SourceWarehouse.WarehouseAddress.Line1,
                        Line2 = transaction.SourceWarehouse.WarehouseAddress.Line2,
                        City = transaction.SourceWarehouse.WarehouseAddress.City,
                        State = transaction.SourceWarehouse.WarehouseAddress.State,
                        PostalCode = transaction.SourceWarehouse.WarehouseAddress.PostalCode,
                        Country = transaction.SourceWarehouse.WarehouseAddress.Country,
                    }
                } : null,
                DestinationWarehouse = transaction.DestinationWarehouse != null ? new GetWarehouseResponseDto
                {
                    Id = transaction.DestinationWarehouse.Id,
                    WarehouseName = transaction.DestinationWarehouse.WarehouseName,
                    WarehouseCode = transaction.DestinationWarehouse.WarehouseCode,
                    Capacity = transaction.DestinationWarehouse.Capacity,
                    Address = new AddressResponse
                    {
                        Line1 = transaction.DestinationWarehouse.WarehouseAddress.Line1,
                        Line2 = transaction.DestinationWarehouse.WarehouseAddress.Line2,
                        City = transaction.DestinationWarehouse.WarehouseAddress.City,
                        State = transaction.DestinationWarehouse.WarehouseAddress.State,
                        PostalCode = transaction.DestinationWarehouse.WarehouseAddress.PostalCode,
                        Country = transaction.DestinationWarehouse.WarehouseAddress.Country,
                    }
                } : null,
                Lines = transaction.Lines.Select(transactionLine => new TransactionLineDto
                {
                   ProductId = transactionLine.ProductId,
                   ProductName = transactionLine.Product.ProductName,
                   SKU = transactionLine.Product.SKU,
                   Quantity = transactionLine.Quantity,
                   UnitCost = transactionLine.UnitCost
                }).ToList(),
                Notes = transaction.Notes,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}