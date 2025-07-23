using AutoMapper;
using Inventory.Application.Products.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class CreateProductCommandHandler: IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.CreateProductDto);

        var productId = await _productRepository.AddAsync(product, cancellationToken);
        
        return productId;
    }
}