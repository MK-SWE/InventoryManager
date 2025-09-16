using Inventory.Application.Transactions.Commands;
using Inventory.Application.Transactions.DTOs;
using Inventory.Application.Transactions.Queries;
using Inventory.Shared.DTOs.InventoryTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionType = Inventory.Domain.Enums.TransactionType;

namespace Inventory.API.Controllers.Inventory.InventoryTransactions;

public class TransactionsController: BaseController
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateTransaction([FromBody] InventoryTransactionRequestDto requestDto)
    {
        var dto = new InventoryTransactionDto
        {
            TransactionType = (TransactionType) requestDto.TransactionType,
            ReferenceNumber = requestDto.ReferenceNumber,
            SourceWarehouseId = requestDto.SourceWarehouseId,
            DestinationWarehouseId = requestDto.DestinationWarehouseId,
            SupplierId = requestDto.SupplierId,
            CustomerId = requestDto.CustomerId,
            Notes = requestDto.Notes,
            Lines = requestDto.Lines.Select(line => new InventoryTransactionLineDto
            (
                line.ProductId,
                line.Quantity,
                line.UnitCost
            )).ToList()
        };
        var request = new CreateTransactionCommand(dto);
        var transactionId = await _mediator.Send(request);
        return CreatedAtAction(
            nameof(GetTransactionById),
            new { transactionId },
            new { Id = transactionId });
    }
    
    [HttpGet("{transactionId:int}")]
    [ProducesResponseType(/*typeof(InventoryTransactionDto),*/ StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult/*<InventoryTransactionDto>*/> GetTransactionById(int transactionId)
    {
        var query = new GetTransactionByIdQuery(transactionId);
        var transaction = await _mediator.Send(query);
        return Ok(transaction);
    }
}