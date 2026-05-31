using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Features.News.Dtos;

public sealed record NewsItemDto(
    Guid Id, string Title, string Url, string Source, NewsCategory Category,
    string[] Tags, DateTimeOffset PublishedAt, string? Summary, string? ImageUrl);

public sealed record NewsListDto(
    IReadOnlyList<NewsItemDto> Items, int Total, int Page, int PageSize);
