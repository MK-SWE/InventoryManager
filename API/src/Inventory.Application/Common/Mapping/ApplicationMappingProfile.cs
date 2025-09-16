using AutoMapper;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Warehouses.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Mapping;



public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<CreateProductCommandDto, Product>();
        CreateMap<UpdateProductCommandDto, Product>()
            .ForMember(dest => dest.SKU, opt => opt.MapFrom((src, dest) => src.SKU ?? dest.SKU))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom((src, dest) => src.ProductName ?? dest.ProductName))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom((src, dest) => src.ProductDescription ?? dest.ProductDescription))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom((src, dest) => src.CategoryId ?? dest.CategoryId))
            .ForMember(dest => dest.UnitOfMeasureId, opt => opt.MapFrom((src, dest) => src.UnitOfMeasureId ?? dest.UnitOfMeasureId))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom((src, dest) => src.UnitPrice ?? dest.UnitPrice))
            .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom((src, dest) => src.ReorderLevel ?? dest.ReorderLevel))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom((src, dest) => src.Weight ?? dest.Weight))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom((src, dest) => src.Volume ?? dest.Volume))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom((src, dest) => src.IsActive ?? dest.IsActive));
    }
}