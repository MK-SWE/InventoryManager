using AutoMapper;
using Inventory.Application.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers;

public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, Product>
{
    private readonly IWriteRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IWriteRepository<Product> productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            request.UpdateProduct.Id = request.Id;
            var product = _mapper.Map<Product>(request.UpdateProduct);
            return await _productRepository.UpdateByIdAsync(request.Id, product);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Product {request.Id} not found", ex);
        }
    }
}