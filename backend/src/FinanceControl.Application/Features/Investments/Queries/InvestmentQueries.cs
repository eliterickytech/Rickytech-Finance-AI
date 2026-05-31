using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Investments.Dtos;
using FinanceControl.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Investments.Queries;

public sealed record GetInvestmentByIdQuery(Guid Id) : IRequest<InvestmentDto>;

public sealed class GetInvestmentByIdHandler : IRequestHandler<GetInvestmentByIdQuery, InvestmentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IQuoteProvider _quotes;

    public GetInvestmentByIdHandler(IApplicationDbContext db, IMapper mapper, IQuoteProvider quotes)
    {
        _db = db; _mapper = mapper; _quotes = quotes;
    }

    public async Task<InvestmentDto> Handle(GetInvestmentByIdQuery r, CancellationToken ct)
    {
        var inv = await _db.Investments.AsNoTracking().FirstOrDefaultAsync(i => i.Id == r.Id, ct)
            ?? throw new NotFoundException("Investment", r.Id);

        var dto = _mapper.Map<InvestmentDto>(inv);
        var quote = await _quotes.GetLatestAsync(inv.Ticker, inv.Currency, ct);

        if (quote is null) return dto;

        var currentValue = quote.Price * inv.Quantity;
        var totalCost = inv.AveragePrice * inv.Quantity;
        var pl = currentValue - totalCost;
        var plPct = totalCost > 0 ? (pl / totalCost) * 100 : 0;

        return dto with { CurrentPrice = quote.Price, CurrentValue = currentValue, ProfitLoss = pl, ProfitLossPercent = plPct };
    }
}

public sealed record GetInvestmentsQuery(InvestmentType? Type = null, Guid? BankId = null) : IRequest<IReadOnlyList<InvestmentDto>>;

public sealed class GetInvestmentsHandler : IRequestHandler<GetInvestmentsQuery, IReadOnlyList<InvestmentDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IQuoteProvider _quotes;

    public GetInvestmentsHandler(IApplicationDbContext db, IMapper mapper, IQuoteProvider quotes)
    {
        _db = db; _mapper = mapper; _quotes = quotes;
    }

    public async Task<IReadOnlyList<InvestmentDto>> Handle(GetInvestmentsQuery r, CancellationToken ct)
    {
        var q = _db.Investments.AsNoTracking();
        if (r.Type is { } t) q = q.Where(i => i.Type == t);
        if (r.BankId is { } b) q = q.Where(i => i.BankId == b);

        var list = await q.OrderBy(i => i.Ticker).ToListAsync(ct);
        var result = new List<InvestmentDto>(list.Count);

        foreach (var inv in list)
        {
            var dto = _mapper.Map<InvestmentDto>(inv);
            var quote = await _quotes.GetLatestAsync(inv.Ticker, inv.Currency, ct);
            if (quote is not null)
            {
                var currentValue = quote.Price * inv.Quantity;
                var totalCost = inv.AveragePrice * inv.Quantity;
                var pl = currentValue - totalCost;
                var plPct = totalCost > 0 ? (pl / totalCost) * 100 : 0;
                dto = dto with { CurrentPrice = quote.Price, CurrentValue = currentValue, ProfitLoss = pl, ProfitLossPercent = plPct };
            }
            result.Add(dto);
        }
        return result;
    }
}

public sealed record GetPortfolioSummaryQuery : IRequest<PortfolioSummaryDto>;

public sealed class GetPortfolioSummaryHandler : IRequestHandler<GetPortfolioSummaryQuery, PortfolioSummaryDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IQuoteProvider _quotes;

    public GetPortfolioSummaryHandler(IApplicationDbContext db, IQuoteProvider quotes) { _db = db; _quotes = quotes; }

    public async Task<PortfolioSummaryDto> Handle(GetPortfolioSummaryQuery r, CancellationToken ct)
    {
        var all = await _db.Investments.AsNoTracking().ToListAsync(ct);
        decimal totalInvested = 0, currentValue = 0;
        var byType = new Dictionary<InvestmentType, (decimal invested, decimal value)>();

        foreach (var inv in all)
        {
            var cost = inv.AveragePrice * inv.Quantity;
            var q = await _quotes.GetLatestAsync(inv.Ticker, inv.Currency, ct);
            var value = (q?.Price ?? inv.AveragePrice) * inv.Quantity;

            totalInvested += cost;
            currentValue += value;

            byType.TryGetValue(inv.Type, out var t);
            byType[inv.Type] = (t.invested + cost, t.value + value);
        }

        var pl = currentValue - totalInvested;
        var plPct = totalInvested > 0 ? (pl / totalInvested) * 100 : 0;

        var typeList = byType.Select(kv => new PortfolioByTypeDto(
            kv.Key, kv.Value.invested, kv.Value.value, kv.Value.value - kv.Value.invested)).ToList();

        return new PortfolioSummaryDto(totalInvested, currentValue, pl, plPct, typeList);
    }
}
