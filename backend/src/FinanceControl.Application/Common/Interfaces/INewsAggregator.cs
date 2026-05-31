using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Common.Interfaces;

public interface INewsAggregator
{
    Task<IReadOnlyList<NewsItem>> FetchAllAsync(CancellationToken ct);
}
