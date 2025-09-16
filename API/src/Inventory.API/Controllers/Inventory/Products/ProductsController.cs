using AutoMapper;
using Inventory.Application.Products.Commands;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Products.Queries;
using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Products;
using Inventory.Shared.Enums;
using Inventory.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers.Inventory.Products;

public class ProductsController: BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Create new product in the database
    /// </summary>
    /// <param name="productDto">The product data to create</param>
    /// <returns>New created product id</returns>
    [HttpPost]
    [ProducesResponseType( typeof(CreateEntityResult) ,StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateEntityResult>> CreateNewProduct([FromBody] CreateProductRequestDto productDto)
    {
        var product = _mapper.Map<CreateProductCommandDto>(productDto);
        var request = new CreateProductCommand(product);
        var productId = await _mediator.Send(request);
        return CreatedAtRoute(
            nameof(GetProductById), 
            routeValues: new { productId },
            value: new CreateEntityResult
            {
                OperationStatus = nameof(SystemOperationStatus.Completed),
                Result = "Product created successfully",
                NewEntityId = productId,
            });
    }
    
    /// <summary>
    /// Get All Products in the database
    /// </summary>
    /// <returns>A List of Products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GetProductsResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllProducts([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var request = new GetAllProductsQuery(pageNumber, pageSize);
        (IReadOnlyList<GetProductsResponseDto> Items, int TotalCount) response = await _mediator.Send(request);
        var products = _mapper.Map<List<GetProductsResponseDto>>(response.Items);
        return Ok(new
        {
            products,
            response.TotalCount
        });
    }
    
    /// <summary>
    /// Get a product by its id 
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <returns>A single Product or Error not found</returns>
    [HttpGet("{productId:int}", Name = "GetProductById")]
    [ProducesResponseType(typeof(GetProductsResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetProductsResponseDto>> GetProductById([FromRoute] int productId)
    {
        var request = new GetProductQuery(productId);
        var response = await _mediator.Send(request);
        if (response == null)
            return NotFound();
        return Ok(response);
    }
    
    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="productId">The product id to delete </param>
    /// <param name="updateDto">The updating object</param>
    /// <returns>The product details after updating</returns>
    [HttpPut("{productId:int}", Name = "UpdateProduct")]
    [ProducesResponseType(typeof(GetProductsResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SystemOperationResult>> UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductRequestDto updateDto)
    {
        var productDto = _mapper.Map<UpdateProductCommandDto>(updateDto);
        var request = new UpdateProductCommand(productId, productDto);
        var product = await _mediator.Send(request);
        var response = _mapper.Map<GetProductsResponseDto>(product);
        return Ok(new SystemOperationResult
        {
            OperationStatus = nameof(SystemOperationStatus.Completed),
            Result = "Product updated successfully",
            Payload = response
        });
    }
    
    /// <summary>
    /// Delete product from database
    /// </summary>
    /// <param name="productId">The product id to delete </param>
    /// <returns>No content</returns>
    [HttpDelete("{productId:int}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SystemOperationResult>> DeleteProduct([FromRoute] int productId)
    {
        var request = new DeleteProductCommand(productId);
        await _mediator.Send(request);
        return Ok(new SystemOperationResult
        {
            OperationStatus = nameof(SystemOperationStatus.Completed),
            Result = "Product deleted successfully",
        });
    }
}
