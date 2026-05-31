using AutoMapper;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.News.Dtos;
using FinanceControl.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.News.Queries;

public sealed record GetNewsQuery(
    NewsCategory? Category = null,
    string? Tag = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 20) : IRequest<NewsListDto>;

public sealed class GetNewsHandler : IRequestHandler<GetNewsQuery, NewsListDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetNewsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<NewsListDto> Handle(GetNewsQuery r, CancellationToken ct)
    {
        var q = _db.NewsItems.AsNoTracking();

        if (r.Category is { } c) q = q.Where(n => n.Category == c);
        if (!string.IsNullOrWhiteSpace(r.Tag))
            q = q.Where(n => n.Tags.Contains(r.Tag));
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var term = r.Search.ToLower();
            q = q.Where(n => n.Title.ToLower().Contains(term) || (n.Summary != null && n.Summary.ToLower().Contains(term)));
        }

        var total = await q.CountAsync(ct);
        var pageSize = Math.Clamp(r.PageSize, 1, 100);
        var skip = Math.Max(0, (r.Page - 1) * pageSize);

        var items = await q.OrderByDescending(n => n.PublishedAt)
            .Skip(skip).Take(pageSize).ToListAsync(ct);

        return new NewsListDto(_mapper.Map<List<NewsItemDto>>(items), total, r.Page, pageSize);
    }
}
