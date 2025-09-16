using Inventory.Application.Common.Exceptions;
using Inventory.Application.Transactions.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Shared.DTOs.InventoryTransactions;
using Inventory.Shared.DTOs.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransactionType = Inventory.Shared.Enums.TransactionType;

namespace Inventory.Application.Transactions.Handlers;

public class GetTransactionByIdQueryHandler: IRequestHandler<GetTransactionByIdQuery, GetTransactionResponseDto>
{
    private readonly IInventoryTransactionRepository _transactionRepository;


    public GetTransactionByIdQueryHandler(IInventoryTransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<GetTransactionResponseDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        {
            var transactionDto = await _transactionRepository.GetByIdWithDetailsAsync( request.Id,cancellationToken);

            if (transactionDto == null)
                throw new NotFoundException("Transaction", $"Transaction with Id: {request.Id} not found");

            return transactionDto;
        }
    }
}






