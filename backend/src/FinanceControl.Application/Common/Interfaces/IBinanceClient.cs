namespace FinanceControl.Application.Common.Interfaces;

public sealed record BinanceBalance(string Asset, decimal Free, decimal Locked);
public sealed record BinanceTradeDto(string Symbol, long Id, decimal Quantity, decimal Price, decimal Commission, string CommissionAsset, bool IsBuyer, DateTimeOffset Time);
public sealed record BinanceTransferDto(string Asset, decimal Amount, DateTimeOffset Time, string Id);

public interface IBinanceClient
{
    Task<bool> TestCredentialsAsync(string apiKey, string apiSecret, CancellationToken ct);
    Task<IReadOnlyList<BinanceBalance>> GetBalancesAsync(string apiKey, string apiSecret, CancellationToken ct);
    Task<IReadOnlyList<BinanceTradeDto>> GetTradesAsync(string apiKey, string apiSecret, string symbol, DateTimeOffset since, CancellationToken ct);
    Task<IReadOnlyList<BinanceTransferDto>> GetDepositsAsync(string apiKey, string apiSecret, DateTimeOffset since, CancellationToken ct);
    Task<IReadOnlyList<BinanceTransferDto>> GetWithdrawalsAsync(string apiKey, string apiSecret, DateTimeOffset since, CancellationToken ct);
    Task<decimal?> GetSpotPriceAsync(string symbol, CancellationToken ct);
}
