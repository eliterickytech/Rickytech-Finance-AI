using AutoMapper;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Banks.Dtos;
using FinanceControl.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Banks.Queries.GetAllBanks;

public sealed record GetAllBanksQuery(
    BankAccountType? Type = null, string? Currency = null, bool? Active = null) : IRequest<IReadOnlyList<BankDto>>;

public sealed class GetAllBanksHandler : IRequestHandler<GetAllBanksQuery, IReadOnlyList<BankDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetAllBanksHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<IReadOnlyList<BankDto>> Handle(GetAllBanksQuery r, CancellationToken ct)
    {
        var q = _db.Banks.AsNoTracking();
        if (r.Type is { } t) q = q.Where(b => b.Type == t);
        if (!string.IsNullOrWhiteSpace(r.Currency)) q = q.Where(b => b.Currency == r.Currency.ToUpper());
        if (r.Active is { } a) q = q.Where(b => b.Active == a);
        var list = await q.OrderBy(b => b.Nickname).ToListAsync(ct);
        return _mapper.Map<List<BankDto>>(list);
    }
}
