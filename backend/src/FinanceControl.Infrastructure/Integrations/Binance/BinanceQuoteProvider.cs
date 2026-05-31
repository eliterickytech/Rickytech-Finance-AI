using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Infrastructure.Integrations.Binance;

/// <summary>
/// IQuoteProvider que tenta primeiro Binance (público) e cai para MockQuoteProvider
/// quando o símbolo não está disponível.
/// </summary>
public sealed class BinanceQuoteProvider : IQuoteProvider
{
    private static readonly Dictionary<string, string> TickerToSymbol = new(StringComparer.OrdinalIgnoreCase)
    {
        ["BTC"] = "BTCUSDT", ["ETH"] = "ETHUSDT", ["ADA"] = "ADAUSDT",
        ["SOL"] = "SOLUSDT", ["BNB"] = "BNBUSDT", ["MATIC"] = "MATICUSDT",
        ["DOT"] = "DOTUSDT", ["AVAX"] = "AVAXUSDT", ["DOGE"] = "DOGEUSDT"
    };

    private readonly IBinanceClient _binance;
    private readonly MockQuoteProvider _fallback;

    public BinanceQuoteProvider(IBinanceClient binance)
    {
        _binance = binance;
        _fallback = new MockQuoteProvider();
    }

    public async Task<AssetQuote?> GetLatestAsync(string ticker, string currency, CancellationToken ct)
    {
        if (TickerToSymbol.TryGetValue(ticker, out var symbol))
        {
            var price = await _binance.GetSpotPriceAsync(symbol, ct);
            if (price is { } p)
                return AssetQuote.Create(ticker, DateOnly.FromDateTime(DateTime.UtcNow), p, "USDT", "Binance");
        }
        return await _fallback.GetLatestAsync(ticker, currency, ct);
    }

    public Task<IReadOnlyList<AssetQuote>> GetHistoryAsync(
        string ticker, string currency, DateOnly from, DateOnly to, CancellationToken ct)
        => _fallback.GetHistoryAsync(ticker, currency, from, to, ct);
}

/// <summary>Re-export para compatibilidade com BinanceQuoteProvider.</summary>
internal sealed class MockQuoteProvider : IQuoteProvider
{
    private readonly Services.MockQuoteProvider _inner = new();
    public Task<AssetQuote?> GetLatestAsync(string ticker, string currency, CancellationToken ct)
        => _inner.GetLatestAsync(ticker, currency, ct);
    public Task<IReadOnlyList<AssetQuote>> GetHistoryAsync(string ticker, string currency, DateOnly from, DateOnly to, CancellationToken ct)
        => _inner.GetHistoryAsync(ticker, currency, from, to, ct);
}
