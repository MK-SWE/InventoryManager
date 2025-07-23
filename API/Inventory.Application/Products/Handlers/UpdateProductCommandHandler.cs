using AutoMapper;
using Inventory.Application.Products.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null) throw new NotFoundException(nameof(Product), request.Id);
    
            _mapper.Map(request.UpdateProduct, product);
            await _productRepository.UpdateAsync(product, cancellationToken);
            return product;
            
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Product {request.Id} not found", ex);
        }
    }
}