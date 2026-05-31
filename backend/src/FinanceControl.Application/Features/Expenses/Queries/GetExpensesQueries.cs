using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Expenses.Dtos;
using FinanceControl.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Expenses.Queries;

public sealed record GetExpenseByIdQuery(Guid Id) : IRequest<ExpenseDto>;

public sealed class GetExpenseByIdHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetExpenseByIdHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ExpenseDto> Handle(GetExpenseByIdQuery r, CancellationToken ct)
    {
        var expense = await _db.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == r.Id, ct)
            ?? throw new NotFoundException("Expense", r.Id);
        return _mapper.Map<ExpenseDto>(expense);
    }
}

public sealed record GetExpensesQuery(
    DateOnly? StartDate = null, DateOnly? EndDate = null,
    Guid? CategoryId = null, Guid? BankId = null, PaymentMethod? PaymentMethod = null,
    decimal? MinAmount = null, decimal? MaxAmount = null, bool? Confirmed = null,
    int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<ExpenseDto>>;

public sealed class GetExpensesHandler : IRequestHandler<GetExpensesQuery, IReadOnlyList<ExpenseDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetExpensesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<IReadOnlyList<ExpenseDto>> Handle(GetExpensesQuery r, CancellationToken ct)
    {
        var q = _db.Expenses.AsNoTracking();
        if (r.StartDate is { } s) q = q.Where(e => e.Date >= s);
        if (r.EndDate is { } e) q = q.Where(x => x.Date <= e);
        if (r.CategoryId is { } c) q = q.Where(e => e.CategoryId == c);
        if (r.BankId is { } b) q = q.Where(e => e.BankId == b);
        if (r.PaymentMethod is { } p) q = q.Where(e => e.PaymentMethod == p);
        if (r.MinAmount is { } min) q = q.Where(e => e.Amount >= min);
        if (r.MaxAmount is { } max) q = q.Where(e => e.Amount <= max);
        if (r.Confirmed is { } cf) q = q.Where(e => e.Confirmed == cf);

        var pageSize = Math.Clamp(r.PageSize, 1, 200);
        var skip = Math.Max(0, (r.Page - 1) * pageSize);
        var list = await q.OrderByDescending(e => e.Date).Skip(skip).Take(pageSize).ToListAsync(ct);
        return _mapper.Map<List<ExpenseDto>>(list);
    }
}
