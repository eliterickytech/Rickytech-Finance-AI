using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Expenses.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Expenses.Commands;

public sealed record CreateExpenseCommand(
    string Description, decimal Amount, DateOnly Date, Guid CategoryId, Guid BankId,
    PaymentMethod PaymentMethod, string[]? Tags,
    RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd) : IRequest<ExpenseDto>;

public sealed class CreateExpenseValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.BankId).NotEmpty();
        RuleFor(x => x.PaymentMethod).IsInEnum();
    }
}

public sealed class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, ExpenseDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateExpenseHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ExpenseDto> Handle(CreateExpenseCommand r, CancellationToken ct)
    {
        var expense = Expense.Create(r.Description, r.Amount, r.Date, r.CategoryId, r.BankId,
            r.PaymentMethod, r.Tags, r.Recurrence, r.RecurrenceEnd);
        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<ExpenseDto>(expense);
    }
}

public sealed record UpdateExpenseCommand(
    Guid Id, string Description, decimal Amount, DateOnly Date, Guid CategoryId, Guid BankId,
    PaymentMethod PaymentMethod, string[]? Tags,
    RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd) : IRequest<ExpenseDto>;

public sealed class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, ExpenseDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public UpdateExpenseHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ExpenseDto> Handle(UpdateExpenseCommand r, CancellationToken ct)
    {
        var expense = await _db.Expenses.FirstOrDefaultAsync(e => e.Id == r.Id, ct)
            ?? throw new NotFoundException("Expense", r.Id);

        expense.Update(r.Description, r.Amount, r.Date, r.CategoryId, r.BankId, r.Tags, r.Recurrence, r.RecurrenceEnd);
        expense.ChangePaymentMethod(r.PaymentMethod);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<ExpenseDto>(expense);
    }
}

public sealed record DeleteExpenseCommand(Guid Id) : IRequest;

public sealed class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand>
{
    private readonly IApplicationDbContext _db;
    public DeleteExpenseHandler(IApplicationDbContext db) => _db = db;

    public async Task Handle(DeleteExpenseCommand r, CancellationToken ct)
    {
        var expense = await _db.Expenses.FirstOrDefaultAsync(e => e.Id == r.Id, ct)
            ?? throw new NotFoundException("Expense", r.Id);
        expense.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }
}
