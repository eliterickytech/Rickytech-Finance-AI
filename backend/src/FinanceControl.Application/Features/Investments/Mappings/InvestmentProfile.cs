using AutoMapper;
using FinanceControl.Application.Features.Investments.Dtos;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Features.Investments.Mappings;

public sealed class InvestmentProfile : Profile
{
    public InvestmentProfile()
    {
        CreateMap<Investment, InvestmentDto>()
            .ForCtorParam("CurrentPrice", opt => opt.MapFrom(_ => (decimal?)null))
            .ForCtorParam("CurrentValue", opt => opt.MapFrom(_ => (decimal?)null))
            .ForCtorParam("ProfitLoss", opt => opt.MapFrom(_ => (decimal?)null))
            .ForCtorParam("ProfitLossPercent", opt => opt.MapFrom(_ => (decimal?)null));
    }
}
