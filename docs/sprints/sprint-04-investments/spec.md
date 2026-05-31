# Sprint 4 - Spec técnica (Investimentos)

## Entidades

```csharp
public sealed class Investment : BaseEntity
{
    public string Ticker { get; private set; }          // BTC, ETH, ADA, SOL, PETR4, IVVB11, HGLG11, CDB-ITAU-2030
    public InvestmentType Type { get; private set; }
    public decimal Quantity { get; private set; }        // decimal(28,18)
    public decimal AveragePrice { get; private set; }    // moeda da corretora
    public string Currency { get; private set; }         // BRL, USD, USDT
    public DateOnly AcquiredAt { get; private set; }
    public Guid BankId { get; private set; }
    public string? Notes { get; private set; }

    public ICollection<InvestmentOperation> Operations { get; private set; } = [];

    public void ApplyOperation(InvestmentOperation op)
    {
        // média ponderada para Buy; FIFO para Sell
        ...
    }
}

public sealed class InvestmentOperation : BaseEntity
{
    public Guid InvestmentId { get; init; }
    public OperationSide Side { get; init; }   // Buy / Sell
    public decimal Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal Fee { get; init; }
    public DateTimeOffset ExecutedAt { get; init; }
    public Guid? IntegrationSourceId { get; init; }  // populado por integrações
}

public sealed class AssetQuote
{
    public string Ticker { get; init; }
    public DateOnly Date { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; }
    public string Source { get; init; }   // "Binance", "CoinGecko", "Mock"
}
```

## IQuoteProvider

```csharp
public interface IQuoteProvider
{
    Task<AssetQuote?> GetLatestAsync(string ticker, string currency, CancellationToken ct);
    Task<IReadOnlyList<AssetQuote>> GetHistoryAsync(string ticker, string currency, DateOnly from, DateOnly to, CancellationToken ct);
}
```

Mock retorna preços parametrizáveis via configuração (`Quotes:Mock:Prices`).

## Endpoints

| Verbo  | Rota                                          |
|--------|-----------------------------------------------|
| POST   | `/api/v1/investimentos`                       |
| GET    | `/api/v1/investimentos`                       |
| GET    | `/api/v1/investimentos/{id}`                  |
| PUT    | `/api/v1/investimentos/{id}`                  |
| DELETE | `/api/v1/investimentos/{id}`                  |
| POST   | `/api/v1/investimentos/{id}/operacoes`        |
| GET    | `/api/v1/investimentos/resumo`                |

## Resumo do portfolio

```json
{
  "totalInvested": 12345.67,
  "currentValue": 14000.00,
  "profitLoss": 1654.33,
  "profitLossPercent": 13.40,
  "byType": [
    { "type": "Cripto", "currentValue": 8000, "profitLoss": 1200 },
    { "type": "Acao",   "currentValue": 4000, "profitLoss":  300 },
    { "type": "RendaFixa", "currentValue": 2000, "profitLoss": 154 }
  ]
}
```
