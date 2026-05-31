using AutoMapper;
using FinanceControl.Application.Features.News.Dtos;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Features.News.Mappings;

public sealed class NewsProfile : Profile
{
    public NewsProfile() => CreateMap<NewsItem, NewsItemDto>();
}
