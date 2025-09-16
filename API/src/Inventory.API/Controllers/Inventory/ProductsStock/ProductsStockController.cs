using Inventory.Application.InventoryStock.Queries;
using Inventory.Shared.DTOs.ProductsStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers.Inventory.ProductsStock;

[Route("products/{productId:int}/stock")]
public class ProductsStockController: BaseController
{
    private readonly IMediator _mediator;

    public ProductsStockController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // /// <summary>
    // /// Add product stock to warehouse 
    // /// </summary>
    // /// <param name="productId">Product Id</param>
    // /// <param name="productStockInWarehouseDto">Product Stock Data</param>
    // /// <returns>New created stock</returns>
    // /// <exception cref="InvalidOperationException"></exception>
    // [HttpPost]
    // [ProducesResponseType(typeof(ProductStockResponseDto), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult> CreateNewProductStock([FromRoute] int productId,
    //     [FromBody] CreateProductStockInWarehouseDto productStockInWarehouseDto)
    // {
    //     productStockInWarehouseDto.ProductId = productId;
    //     var request = new CreateProductStockInWarehouseCommand(productStockInWarehouseDto);
    //     try
    //     {
    //         var productStockId = await _mediator.Send(request);
    //         return CreatedAtAction(
    //             actionName: nameof(GetByProductIdWithStock),
    //             routeValues: new { productId },
    //             value: new { id = productStockId });
    //     }
    //     catch (InvalidOperationException e)
    //     {
    //         throw new InvalidOperationException("Cannot Create Product Stock", e);
    //     }
    // }
    
    /// <summary>
    /// Get Product With its stock in warehouses
    /// </summary>
    /// <param name="productId">Product Id</param>
    /// <returns>A single Product with stock or Error not found</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    [HttpGet]
    [ProducesResponseType(typeof(ProductWithStocksResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductStockResponseDto>> GetByProductIdWithStock([FromRoute] int productId)
    {
        var request = new GetProductWithStockQuery(productId);
        var product = await _mediator.Send(request);
        return Ok(product);
    }
    
    // /// <summary>
    // /// Update product stock in warehouse
    // /// </summary>
    // /// <param name="productId">The product we want to adjust its quantity</param>
    // /// <param name="warehouseId">The warehouse Id where the product located</param>
    // /// <param name="stockDto">New Product Stock</param>
    // /// <returns>No Content</returns>
    // [HttpPut("{warehouseId:int}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> UpdateStock( [FromRoute] int productId, [FromRoute] int warehouseId, [FromBody] UpdateProductStockDto stockDto)
    // {
    //     var command = new UpdateProductStockInWarehouseCommand(productId, warehouseId, stockDto.NewQuantity, stockDto.StockStatus);
    //     await _mediator.Send(command);
    //     return NoContent();
    // }
    
    // /// <summary>
    // /// Remove stock from warehouse
    // /// </summary>
    // /// <param name="productId">The product we want to delete its quantity</param>
    // /// <param name="warehouseId">The warehouse Id where the product located</param>
    // /// <returns>No Content</returns>
    // [HttpDelete("{warehouseId:int}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> RemoveStock( [FromRoute] int productId, [FromRoute] int warehouseId)
    // {
    //     var command = new RemoveProductStockFromWarehouseCommand(productId, warehouseId);
    //     await _mediator.Send(command);
    //     return NoContent();
    // }
    
    // /// <summary>
    // /// Transfer stock between warehouses
    // /// </summary>
    // /// <param name="productId">The ProductId we want to transfer it's stock</param>
    // /// <returns>No Content</returns>
    // /*/// <param name="transferDto">The update object contains the source WarehouseId, the destination WarehouseId and the amount</param>*/
    // [HttpPost("transfer")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public Task<IActionResult> TransferStock(
    //     [FromRoute] int productId)
    // {
    //     throw new NotImplementedException();
    // }
}
