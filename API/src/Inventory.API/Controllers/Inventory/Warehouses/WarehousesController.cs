using Inventory.Application.Warehouses.Commands;
using Inventory.Application.Warehouses.DTOs;
using Inventory.Application.Warehouses.Queries;
using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Warehouses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers.Inventory.Warehouses;

public class WarehousesController: BaseController
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create new warehouse in the database
    /// </summary>
    /// <param name="warehouseDto">The warehouse data to create</param>
    /// <returns>New created warehouse id</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateNewWarehouse([FromBody] CreateWarehouseDto warehouseDto)
    {
        var request = new CreateWarehouseCommand(warehouseDto);
        int warehouseId = await _mediator.Send(request);
        return CreatedAtRoute(
            nameof(GetWarehouseById), 
            routeValues: new { id = warehouseId },
            value: new { Id = warehouseId });
    }
    
    /// <summary>
    /// Get All Warehouses in the database
    /// </summary>
    /// <returns>A List of Warehouses</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GetWarehouseResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<Warehouse>>> GetAllWarehouses()
    {
        var request = new GetAllWarehousesQuery();
        IReadOnlyList<GetWarehouseResponseDto> warehouses = await _mediator.Send(request);
        return Ok(warehouses);
    }
    
    /// <summary>
    /// Get a warehouse by its id 
    /// </summary>
    /// <param name="id">The warehouse id</param>
    /// <returns>A single Warehouse or Error not found</returns>
    [HttpGet("{id:int}", Name = "GetWarehouseById")]
    [ProducesResponseType(typeof(GetWarehouseResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetWarehouseResponseDto>> GetWarehouseById([FromRoute] int id)
    {
        var request = new GetWarehouseQuery(id);
        var warehouse = await _mediator.Send(request);
        return Ok(warehouse);
    }

    /// <summary>
    /// Get a warehouse with its stock by its id
    /// </summary>
    /// <param name="id">The warehouse id</param>
    /// <returns>A single Warehouse with its stock or Error not found </returns>
    [HttpGet("{id:int}/stock", Name = "GetWarehouseStockById")]
    [ProducesResponseType(typeof(GetWarehouseResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetWarehouseWithStockResponseDto?>> GetWarehouseStockById(int id)
    {
        var request = new GetWarehouseStockQuery(id);
        var response = await _mediator.Send(request);
        return Ok(response);
    }
    
    /// <summary>
    /// Delete warehouse from database
    /// </summary>
    /// <param name="id">The warehouse id to delete </param>
    /// <param name="warehouseDto">The updating object</param>
    /// <returns>No content</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Warehouse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Warehouse>> UpdateWarehouse([FromRoute] int id, [FromBody] UpdateWarehouseDto warehouseDto)
    {
        var request = new UpdateWarehouseCommand(id, warehouseDto);
        var warehouse = await _mediator.Send(request);
        return Ok(warehouse);
    }
    
    /// <summary>
    /// Delete warehouse from database
    /// </summary>
    /// <param name="id">The warehouse id to delete </param>
    /// <returns>No content</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteWarehouse([FromRoute] int id)
    {
        var request = new DeleteWarehouseCommand(id);
        await _mediator.Send(request);
        return NoContent();
    }
}
