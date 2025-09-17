using Inventory.Application.Common.Interfaces;
using Inventory.Application.Transactions.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.Transactions.Services;

public sealed class InventoryTransactionService : IInventoryTransactionService
{
    private readonly IInventoryStockService _inventoryStockService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<InventoryTransaction> _transactionRepo;

    public InventoryTransactionService(
        IInventoryStockService inventoryStockService,
        IUnitOfWork unitOfWork,
        IRepository<InventoryTransaction> transactionRepo)
    {
        _inventoryStockService = inventoryStockService;
        _unitOfWork = unitOfWork;
        _transactionRepo = transactionRepo;
    }
    public async Task<int> ProcessTransactionAsync(CreateTransactionCommand command, CancellationToken cancellationToken = default)
    {
        var transaction = MapToDomain(command);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            switch (transaction.TransactionType)
            {
                case TransactionType.PurchaseReceipt:
                case TransactionType.Return:
                    await _inventoryStockService.ReceiveStock(transaction, cancellationToken);
                    break;

                case TransactionType.SalesFulfillment:
                    await _inventoryStockService.ShipStock(transaction, cancellationToken);
                    break;

                case TransactionType.StockTransfer:
                    await _inventoryStockService.StockTransfer(transaction, cancellationToken);
                    break;

                case TransactionType.StockAdjustment:
                    await _inventoryStockService.AdjustStock(transaction, cancellationToken);
                    break;
                
                //ToDo: Implement Production Input/OutPut Operations
                case TransactionType.ProductionInput:
                case TransactionType.ProductionOutput:
                    throw new InvalidStockOperationException("Transaction",
                        $"Not Implemented transaction type: {transaction.TransactionType}");

                default:
                    throw new InvalidStockOperationException("Transaction",
                        $"Unsupported transaction type: {transaction.TransactionType}");
            }

            var transactionId = await _transactionRepo.AddAsync(transaction, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return transactionId;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    #region Helper Methods
    private static InventoryTransaction MapToDomain(CreateTransactionCommand command)
    {
        var transaction = InventoryTransaction.Create(
            command.InventoryTransactionDto.TransactionType,
            command.InventoryTransactionDto.ReferenceNumber,
            command.InventoryTransactionDto.DestinationWarehouseId ?? 0,
            command.InventoryTransactionDto.SourceWarehouseId ?? 0,
            command.InventoryTransactionDto.Notes ?? ""
        );

        foreach (var line in command.InventoryTransactionDto.Lines)
        {
            transaction.AddLine(line.ProductId, line.Quantity, line.UnitCost);
        }

        return transaction;
    }
    
    #endregion
    
}