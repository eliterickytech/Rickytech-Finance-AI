using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Infrastructure.Services;

/// <summary>
/// Implementação mock do IQuoteProvider. Substituída pelo BinanceQuoteProvider no Sprint 6.
/// </summary>
public sealed class MockQuoteProvider : IQuoteProvider
{
    private static readonly Dictionary<string, decimal> MockPrices = new(StringComparer.OrdinalIgnoreCase)
    {
        ["BTC"] = 68_000m, ["ETH"] = 3_500m, ["ADA"] = 0.45m, ["SOL"] = 145m,
        ["BNB"] = 580m, ["USDT"] = 1m, ["USDC"] = 1m, ["MATIC"] = 0.62m,
        ["DOT"] = 6.8m, ["AVAX"] = 32m, ["DOGE"] = 0.12m,
        ["PETR4"] = 38m, ["VALE3"] = 65m, ["IVVB11"] = 320m, ["HGLG11"] = 165m
    };

    public Task<AssetQuote?> GetLatestAsync(string ticker, string currency, CancellationToken ct)
    {
        if (!MockPrices.TryGetValue(ticker.ToUpperInvariant(), out var price))
            return Task.FromResult<AssetQuote?>(null);

        var quote = AssetQuote.Create(ticker, DateOnly.FromDateTime(DateTime.UtcNow), price, currency, "Mock");
        return Task.FromResult<AssetQuote?>(quote);
    }

    public Task<IReadOnlyList<AssetQuote>> GetHistoryAsync(
        string ticker, string currency, DateOnly from, DateOnly to, CancellationToken ct)
    {
        if (!MockPrices.TryGetValue(ticker.ToUpperInvariant(), out var price))
            return Task.FromResult<IReadOnlyList<AssetQuote>>(Array.Empty<AssetQuote>());

        var list = new List<AssetQuote>();
        for (var d = from; d <= to; d = d.AddDays(1))
        {
            // pequena variação determinística para teste
            var factor = 1 + ((d.DayNumber % 10) - 5) * 0.005m;
            list.Add(AssetQuote.Create(ticker, d, price * factor, currency, "Mock"));
        }
        return Task.FromResult<IReadOnlyList<AssetQuote>>(list);
    }
}
