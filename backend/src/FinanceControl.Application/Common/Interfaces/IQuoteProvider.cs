using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Common.Interfaces;

public interface IQuoteProvider
{
    Task<AssetQuote?> GetLatestAsync(string ticker, string currency, CancellationToken ct);
    Task<IReadOnlyList<AssetQuote>> GetHistoryAsync(string ticker, string currency, DateOnly from, DateOnly to, CancellationToken ct);
}
