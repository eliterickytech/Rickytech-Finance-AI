using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Incomes.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Incomes.Queries.GetIncomes;

public sealed record GetIncomeByIdQuery(Guid Id) : IRequest<IncomeDto>;

public sealed class GetIncomeByIdHandler : IRequestHandler<GetIncomeByIdQuery, IncomeDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetIncomeByIdHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<IncomeDto> Handle(GetIncomeByIdQuery r, CancellationToken ct)
    {
        var income = await _db.Incomes.AsNoTracking().FirstOrDefaultAsync(i => i.Id == r.Id, ct)
            ?? throw new NotFoundException("Income", r.Id);
        return _mapper.Map<IncomeDto>(income);
    }
}

public sealed record GetIncomesQuery(
    DateOnly? StartDate = null, DateOnly? EndDate = null,
    Guid? CategoryId = null, Guid? BankId = null,
    decimal? MinAmount = null, decimal? MaxAmount = null,
    bool? Confirmed = null,
    int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<IncomeDto>>;

public sealed class GetIncomesHandler : IRequestHandler<GetIncomesQuery, IReadOnlyList<IncomeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetIncomesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<IReadOnlyList<IncomeDto>> Handle(GetIncomesQuery r, CancellationToken ct)
    {
        var q = _db.Incomes.AsNoTracking();
        if (r.StartDate is { } s) q = q.Where(i => i.Date >= s);
        if (r.EndDate is { } e) q = q.Where(i => i.Date <= e);
        if (r.CategoryId is { } c) q = q.Where(i => i.CategoryId == c);
        if (r.BankId is { } b) q = q.Where(i => i.BankId == b);
        if (r.MinAmount is { } min) q = q.Where(i => i.Amount >= min);
        if (r.MaxAmount is { } max) q = q.Where(i => i.Amount <= max);
        if (r.Confirmed is { } cf) q = q.Where(i => i.Confirmed == cf);

        var pageSize = Math.Clamp(r.PageSize, 1, 200);
        var skip = Math.Max(0, (r.Page - 1) * pageSize);
        var list = await q.OrderByDescending(i => i.Date).Skip(skip).Take(pageSize).ToListAsync(ct);
        return _mapper.Map<List<IncomeDto>>(list);
    }
}
