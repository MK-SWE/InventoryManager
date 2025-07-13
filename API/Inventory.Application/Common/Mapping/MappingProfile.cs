using AutoMapper;
using Inventory.Application.Products.DTOs;
using Inventory.Application.Warehouses.DTOs;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence.Repositories.HelperMethods;

namespace Inventory.Application.Common.Mapping;



public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductDTO, Product>();
        CreateMap<UpdateProductDTO, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !srcMember.Equals(GetDefaultValues.GetValue(srcMember.GetType()))));
        CreateMap<CreateWarehouseDTO, Warehouse>();
        CreateMap<UpdateWarehouseDTO, Warehouse>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !srcMember.Equals(GetDefaultValues.GetValue(srcMember.GetType()))));
    }
}
