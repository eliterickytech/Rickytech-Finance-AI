using AutoMapper;
using FinanceControl.Application.Features.Categories.Dtos;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Features.Categories.Mappings;

public sealed class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForCtorParam("Color", opt => opt.MapFrom(src => src.Color.Value));
    }
}
