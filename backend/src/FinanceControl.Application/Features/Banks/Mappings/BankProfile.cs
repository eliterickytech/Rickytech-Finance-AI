using AutoMapper;
using FinanceControl.Application.Features.Banks.Dtos;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Features.Banks.Mappings;

public sealed class BankProfile : Profile
{
    public BankProfile()
    {
        CreateMap<Bank, BankDto>()
            .ForCtorParam("CurrentBalance", opt => opt.MapFrom(src => src.OpeningBalance)); // CurrentBalance calculado via IBankBalanceCalculator no Sprint 3
    }
}
