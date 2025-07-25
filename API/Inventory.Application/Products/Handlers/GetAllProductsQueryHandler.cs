﻿using Inventory.Application.Products.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class GetAllProductsQueryHandler: IRequestHandler<GetAllProductsQuery, IReadOnlyList<Product>>
{
    private readonly IProductRepository _productsRepository;

    public GetAllProductsQueryHandler(IProductRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<IReadOnlyList<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<Product> products = await _productsRepository.GetAllAsync(cancellationToken);
        return products;
    }
}