using AutoMapper;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Categories.Dtos;
using FinanceControl.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery(
    CategoryType? Type = null,
    bool? Active = null,
    string? Search = null) : IRequest<IReadOnlyList<CategoryDto>>;

public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
    {
        var query = _db.Categories.AsNoTracking();

        if (request.Type is { } type)
            query = query.Where(c => c.Type == type || c.Type == CategoryType.Both);

        if (request.Active is { } active)
            query = query.Where(c => c.Active == active);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(term));
        }

        var list = await query.OrderBy(c => c.Name).ToListAsync(ct);
        return _mapper.Map<List<CategoryDto>>(list);
    }
}
