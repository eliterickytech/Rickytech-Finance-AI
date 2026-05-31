# Sprint 6 - Spec técnica (Binance)

## Entidade

```csharp
public sealed class IntegrationConfig : BaseEntity
{
    public IntegrationProvider Provider { get; private set; }   // Binance, OpenFinance, Custom
    public string ApiKeyEncrypted { get; private set; }
    public string? ApiSecretEncrypted { get; private set; }
    public bool Active { get; private set; }
    public DateTimeOffset? LastSyncAt { get; private set; }
    public IDictionary<string, string> Metadata { get; private set; } = new Dictionary<string, string>();
}
```

`ApiKeyEncrypted`/`ApiSecretEncrypted` armazenam payload base64 do AES-256-GCM
(chave mestra em `appsettings` → `KeyVault:MasterKey`, nunca em git).

## IBinanceClient

```csharp
public interface IBinanceClient
{
    Task<BinanceAccount> GetAccountAsync(CancellationToken ct);
    Task<IReadOnlyList<BinanceTrade>> GetMyTradesAsync(string symbol, DateTimeOffset since, CancellationToken ct);
    Task<IReadOnlyList<BinanceDeposit>> GetDepositsAsync(DateTimeOffset since, CancellationToken ct);
    Task<IReadOnlyList<BinanceWithdraw>> GetWithdrawsAsync(DateTimeOffset since, CancellationToken ct);
    Task<decimal> GetSpotPriceAsync(string symbol, CancellationToken ct);   // BTCUSDT, ETHUSDT, ADAUSDT, SOLUSDT
}
```

## Mapeamento de símbolos

| Binance | Finance Control Ticker | Currency |
|---------|------------------------|----------|
| BTCUSDT | BTC                    | USDT     |
| ETHUSDT | ETH                    | USDT     |
| ADAUSDT | ADA                    | USDT     |
| SOLUSDT | SOL                    | USDT     |
| BNBUSDT | BNB                    | USDT     |
| BTCBRL  | BTC                    | BRL      |
| USDTBRL | USDT                   | BRL      |

## Pipeline Sync (resumo)

```
1. ValidateCreds() → GET /api/v3/account
2. UpsertBank(Binance, currency=USDT)
3. para cada balance > 0:
       UpsertInvestment(ticker, quantity=free+locked, type=Cripto, bankId=BinanceBank)
4. para cada par com saldo:
       GetMyTrades(since=LastSyncAt) → criar InvestmentOperation
5. GetDeposits(since=LastSyncAt) → criar Income (categoria "Depósito Cripto")
6. GetWithdraws(since=LastSyncAt) → criar Expense (categoria "Saque Cripto")
7. Update LastSyncAt
```

Cada item carrega `IntegrationSourceId = "BINANCE:<tradeId/depositId/withdrawId>"`
para garantir idempotência.

## Segurança

- Chaves Binance criadas com permissão **Read-Only** (validar via header obrigatório no health endpoint).
- Master key em User Secrets (dev) e Key Vault (prod).
- Logs de sync **nunca** logam `ApiSecret` (filtro Serilog).
