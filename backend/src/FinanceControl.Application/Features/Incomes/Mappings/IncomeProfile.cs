using AutoMapper;
using FinanceControl.Application.Features.Incomes.Dtos;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Features.Incomes.Mappings;

public sealed class IncomeProfile : Profile
{
    public IncomeProfile() => CreateMap<Income, IncomeDto>();
}
