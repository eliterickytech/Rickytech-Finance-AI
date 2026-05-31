using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Incomes.Dtos;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Incomes.Commands.UpdateIncome;

public sealed record UpdateIncomeCommand(
    Guid Id, string Description, decimal Amount, DateOnly Date, Guid CategoryId, Guid BankId,
    string[]? Tags, RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd) : IRequest<IncomeDto>;

public sealed class UpdateIncomeValidator : AbstractValidator<UpdateIncomeCommand>
{
    public UpdateIncomeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.BankId).NotEmpty();
    }
}

public sealed class UpdateIncomeHandler : IRequestHandler<UpdateIncomeCommand, IncomeDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public UpdateIncomeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<IncomeDto> Handle(UpdateIncomeCommand r, CancellationToken ct)
    {
        var income = await _db.Incomes.FirstOrDefaultAsync(i => i.Id == r.Id, ct)
            ?? throw new NotFoundException("Income", r.Id);

        income.Update(r.Description, r.Amount, r.Date, r.CategoryId, r.BankId, r.Tags, r.Recurrence, r.RecurrenceEnd);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<IncomeDto>(income);
    }
}
