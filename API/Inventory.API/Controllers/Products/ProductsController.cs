using AutoMapper;
using Inventory.Application.Products.Commands;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Products.Queries;
using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers.Products;

public class ProductsController: BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    private record Create(int Id);
    
    /// <summary>
    /// Create new product in the database
    /// </summary>
    /// <param name="productDto">The product data to create</param>
    /// <returns>New created product id</returns>
    [HttpPost]
    [ProducesResponseType( typeof(Create) ,StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateNewProduct([FromBody] CreateProductRequestDto productDto)
    {
        var product = _mapper.Map<CreateProductDto>(productDto);
        var request = new CreateProductCommand(product);
        var productId = await _mediator.Send(request);
        return CreatedAtRoute(
            nameof(GetProductById), 
            routeValues: new { productId },
            value: new { Id = productId });
    }
    
    /// <summary>
    /// Get All Products in the database
    /// </summary>
    /// <returns>A List of Products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GetProductsResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllProducts()
    {
        var request = new GetAllProductsQuery();
        IReadOnlyList<Product> products = await _mediator.Send(request);
        var responseDtos = new GetProductsResponseDto[products.Count];
        for (int i = 0; i < products.Count; i++)
        {
            var product = products[i];
            responseDtos[i] = new GetProductsResponseDto()
            {
                SKU = product.SKU,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                CategoryId = product.CategoryId,
                UnitOfMeasureId = product.UnitOfMeasureId,
                UnitPrice = product.UnitPrice,
                ReorderLevel = product.ReorderLevel,
                Weight = product.Weight,
                Volume = product.Volume,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive
            };
        }
        return Ok(responseDtos);
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
    public async Task<ActionResult<Product>> GetProductById([FromRoute] int productId)
    {
        var request = new GetProductQuery(productId);
        var response = await _mediator.Send(request);
        var product = _mapper.Map<GetProductsResponseDto>(response);
        return Ok(product);
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
    public async Task<ActionResult<Product>> UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductRequestDto updateDto)
    {
        var productDto = _mapper.Map<UpdateProductDto>(updateDto);
        var request = new UpdateProductCommand(productId, productDto);
        var product = await _mediator.Send(request);
        var response = _mapper.Map<GetProductsResponseDto>(product);
        return Ok(response);
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
