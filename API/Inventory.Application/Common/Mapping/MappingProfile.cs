using AutoMapper;
using Inventory.Application.Products.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Mapping;



public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductDTO, Product>();
        CreateMap<UpdateProductDTO, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null && !srcMember.Equals(GetDefaultValue(srcMember.GetType()))));
    }
    
    private static object? GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
