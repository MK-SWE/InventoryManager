using AutoMapper;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Warehouses.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Mapping;



public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>()
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
        
        CreateMap<CreateWarehouseDto, Warehouse>();
        CreateMap<UpdateWarehouseDto, Warehouse>()
            .ForMember(dest => dest.WarehouseCode, opt => opt.MapFrom((src, dest) => src.WarehouseCode ?? dest.WarehouseCode))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom((src, dest) => src.WarehouseName ?? dest.WarehouseName))
            .ForMember(dest => dest.WarehouseAddress, opt => opt.MapFrom((src, dest) => src.WarehouseAddress ?? dest.WarehouseAddress)) 
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom((src, dest) => src.Capacity ?? dest.Capacity))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom((src, dest) => src.IsActive ?? dest.IsActive));
    }
}