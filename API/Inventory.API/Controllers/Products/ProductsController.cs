﻿using Inventory.Application.Products.Commands;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Products.Queries;
using Inventory.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers.Products;

public class ProductsController: BaseController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Create new product in the database
    /// </summary>
    /// <param name="productDto">The product data to create</param>
    /// <returns>New created product id</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateNewProduct([FromBody] CreateProductDto productDto)
    {
        var request = new CreateProductCommand(productDto);
        int productId = await _mediator.Send(request);
        return CreatedAtRoute(
            nameof(GetProductById), 
            routeValues: new { id = productId },
            value: new { Id = productId });
    }
    
    /// <summary>
    /// Get All Products in the database
    /// </summary>
    /// <returns>A List of Products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllProducts()
    {
        var request = new GetAllProductsQuery();
        IReadOnlyList<Product> products = await _mediator.Send(request);
        return Ok(products);
    }
    
    /// <summary>
    /// Get a product by its id 
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <returns>A single Product or Error not found</returns>
    [HttpGet("{productId:int}", Name = "GetProductById")]
    [ProducesResponseType(typeof(Product),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> GetProductById([FromRoute] int productId)
    {
        var request = new GetProductQuery(productId);
        var product = await _mediator.Send(request);
        return Ok(product);
    }
    
    /// <summary>
    /// Delete product from database
    /// </summary>
    /// <param name="productId">The product id to delete </param>
    /// <param name="productDto">The updating object</param>
    /// <returns>No content</returns>
    [HttpPut("{productId:int}", Name = "UpdateProduct")]
    [ProducesResponseType(typeof(Product),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductDto productDto)
    {
        var request = new UpdateProductCommand(productId, productDto);
        var product = await _mediator.Send(request);
        return Ok(product);
    }
    
    /// <summary>
    /// Delete product from database
    /// </summary>
    /// <param name="productId">The product id to delete </param>
    /// <returns>No content</returns>
    [HttpDelete("{productId:int}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteProduct([FromRoute] int productId)
    {
        var request = new DeleteProductCommand(productId);
        await _mediator.Send(request);
        return NoContent();
    }
}
