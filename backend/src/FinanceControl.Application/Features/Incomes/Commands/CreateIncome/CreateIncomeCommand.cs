using AutoMapper;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Incomes.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;

namespace FinanceControl.Application.Features.Incomes.Commands.CreateIncome;

public sealed record CreateIncomeCommand(
    string Description, decimal Amount, DateOnly Date, Guid CategoryId, Guid BankId,
    string[]? Tags, RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd) : IRequest<IncomeDto>;

public sealed class CreateIncomeValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.BankId).NotEmpty();
        RuleFor(x => x.Recurrence).IsInEnum();
    }
}

public sealed class CreateIncomeHandler : IRequestHandler<CreateIncomeCommand, IncomeDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateIncomeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<IncomeDto> Handle(CreateIncomeCommand r, CancellationToken ct)
    {
        var income = Income.Create(r.Description, r.Amount, r.Date, r.CategoryId, r.BankId,
            r.Tags, r.Recurrence, r.RecurrenceEnd);
        _db.Incomes.Add(income);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<IncomeDto>(income);
    }
}
