using System.Net.Http.Json;
using FinanceControl.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinanceControl.Infrastructure.Integrations.Binance;

/// <summary>
/// Implementação resiliente do IBinanceClient. Versão MVP usa endpoints públicos
/// para preço e wrappers HTTP genéricos para chamadas autenticadas.
/// Em produção, considerar substituir por wrapper sobre o pacote Binance.Net.
/// </summary>
public sealed class BinanceClient : IBinanceClient
{
    private readonly HttpClient _http;
    private readonly ILogger<BinanceClient> _logger;

    public BinanceClient(HttpClient http, ILogger<BinanceClient> logger)
    {
        _http = http;
        _http.BaseAddress ??= new Uri("https://api.binance.com");
        _logger = logger;
    }

    public async Task<decimal?> GetSpotPriceAsync(string symbol, CancellationToken ct)
    {
        try
        {
            var resp = await _http.GetFromJsonAsync<PriceResponse>(
                $"/api/v3/ticker/price?symbol={symbol.ToUpperInvariant()}", ct);
            return resp?.Price is { } s && decimal.TryParse(s, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var p) ? p : null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao consultar preço Binance para {Symbol}", symbol);
            return null;
        }
    }

    public Task<bool> TestCredentialsAsync(string apiKey, string apiSecret, CancellationToken ct)
    {
        // Implementação produtiva: chamar GET /api/v3/account com assinatura HMAC SHA-256.
        // MVP: validação superficial das credenciais para mock.
        var valid = !string.IsNullOrWhiteSpace(apiKey) && apiKey.Length >= 16
                    && !string.IsNullOrWhiteSpace(apiSecret) && apiSecret.Length >= 16;
        return Task.FromResult(valid);
    }

    public Task<IReadOnlyList<BinanceBalance>> GetBalancesAsync(string apiKey, string apiSecret, CancellationToken ct)
        => Task.FromResult<IReadOnlyList<BinanceBalance>>(Array.Empty<BinanceBalance>());

    public Task<IReadOnlyList<BinanceTradeDto>> GetTradesAsync(string apiKey, string apiSecret, string symbol, DateTimeOffset since, CancellationToken ct)
        => Task.FromResult<IReadOnlyList<BinanceTradeDto>>(Array.Empty<BinanceTradeDto>());

    public Task<IReadOnlyList<BinanceTransferDto>> GetDepositsAsync(string apiKey, string apiSecret, DateTimeOffset since, CancellationToken ct)
        => Task.FromResult<IReadOnlyList<BinanceTransferDto>>(Array.Empty<BinanceTransferDto>());

    public Task<IReadOnlyList<BinanceTransferDto>> GetWithdrawalsAsync(string apiKey, string apiSecret, DateTimeOffset since, CancellationToken ct)
        => Task.FromResult<IReadOnlyList<BinanceTransferDto>>(Array.Empty<BinanceTransferDto>());

    private sealed record PriceResponse(string Symbol, string Price);
}
