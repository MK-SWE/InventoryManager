using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.Command;
using Inventory.Application.StockReservation.DTOs;
using Inventory.Application.StockReservation.Queries;
using Inventory.Application.StockReservation.ValueObjects;
using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.InventoryStockReservation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers.Inventory.StockReservation;

public class StockReservationController: BaseController
{
    private readonly IMediator _mediator;

    public StockReservationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult<IReservationResult>> CreateReservation([FromBody] CreateReservationCommandDto reserveItems)
    {
        var request = new CreateStockReservationCommand(reserveItems);
        IReservationResult response = await _mediator.Send(request);
        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessage);
        }
        return Ok(response);
    }
    
    [HttpPost("{reservationReference:Guid}/allocate")]
    public async Task<ActionResult<IReservationResult>> AllocateReservation([FromRoute] Guid reservationReference, [FromBody] StockReservationAllocationDto stockReservationAllocationDto)
    {
        var command = new AllocateStockReservationCommand(reservationReference, stockReservationAllocationDto);
        IReservationResult response = await _mediator.Send(command);
        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessage);
        }
        return Ok(response);
    }
    
    [HttpGet("{reservationReference:Guid}")]
    public async Task<ActionResult<InventoryStockReservationResponseDto?>> GetReservation([FromRoute] Guid reservationReference)
    {
        var request = new GetStockReservationQuery(reservationReference);
        InventoryStockReservationResponseDto? response = await _mediator.Send(request);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }
    
    [HttpPatch("{reservationReference:Guid}")]
    public async Task<ActionResult<IReservationResult>> UpdateReservation([FromRoute] Guid reservationReference, [FromBody] UpdateStockReservationCommandDto updateDto)
    {
        var command = new UpdateStockReservationCommand(reservationReference, updateDto);
        IReservationResult response = await _mediator.Send(command);
        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessage);
        }
        return Ok(response);
    }
    
    [HttpDelete("{reservationReference:Guid}")]
    public async Task<ActionResult<IReservationResult>> CancelReservation([FromRoute] Guid reservationReference)
    {
        var command = new CancelStockReservationCommand(reservationReference);
        IReservationResult response = await _mediator.Send(command);
        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessage);
        }
        return Ok(response);
    }
}