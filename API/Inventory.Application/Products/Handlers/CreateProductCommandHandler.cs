using AutoMapper;
using Inventory.Application.Products.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class CreateProductCommandHandler: IRequestHandler<CreateProductCommand, int>
{
    private readonly IWriteRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IWriteRepository<Product> productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.CreateProductDTO);

        var productId = await _productRepository.CreateNewAsync(product);
        
        return productId;
    }
}