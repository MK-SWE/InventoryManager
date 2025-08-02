using AutoMapper;
using Inventory.Application.Products.DTOs;
using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Products;

namespace Inventory.API.Mapping;

public class ApiMappingProfile: Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateProductRequestDto, CreateProductDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
        CreateMap<UpdateProductRequestDto, UpdateProductDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
        CreateMap<Product, GetProductsResponseDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
    }
}