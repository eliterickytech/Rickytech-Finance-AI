using AutoMapper;
using FinanceControl.Application.Features.Expenses.Dtos;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Features.Expenses.Mappings;

public sealed class ExpenseProfile : Profile
{
    public ExpenseProfile() => CreateMap<Expense, ExpenseDto>();
}
