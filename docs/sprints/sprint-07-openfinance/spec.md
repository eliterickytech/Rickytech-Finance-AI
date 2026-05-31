# Sprint 7 - Spec técnica (Open Finance)

## Endpoints

```
POST /api/v1/integracoes/openfinance/consentir
  body: { "cpf": "...", "bankCode": "ITAU" }
  → 200 { "consentUrl": "https://...", "consentId": "..." }

GET  /api/v1/integracoes/openfinance/callback?code=...&state=...
  → 302 redirect para o frontend

POST /api/v1/integracoes/openfinance/sync
  → 200 { "imported": 42, "skipped": 3 }

GET  /api/v1/integracoes/openfinance/status
  → 200 { "connected": true, "lastSyncAt": "..." }
```

## IOpenFinanceClient

```csharp
public interface IOpenFinanceClient
{
    Task<ConsentResponse> CreateConsentAsync(string cpf, string bankCode, CancellationToken ct);
    Task<TokenResponse> ExchangeCodeAsync(string code, string state, CancellationToken ct);
    Task<TokenResponse> RefreshAsync(string refreshToken, CancellationToken ct);
    IAsyncEnumerable<OfiAccount> GetAccountsAsync(string accessToken, CancellationToken ct);
    IAsyncEnumerable<OfiTransaction> GetTransactionsAsync(string accessToken, string accountId, DateOnly since, CancellationToken ct);
}
```

## Mapeamento OFI → Finance Control

| Campo OFI               | Destino                                  |
|-------------------------|------------------------------------------|
| `creditDebitType=CREDIT`| Income                                   |
| `creditDebitType=DEBIT` | Expense                                  |
| `transactionAmount`     | Amount                                   |
| `transactionDate`       | Date                                     |
| `transactionName`       | Description                              |
| `transactionId`         | `IntegrationSourceId = "OFI:<id>"`       |

## Categorização automática (regras default)

```
keywords → category
"PIX RECEBIDO", "TED CREDITO"          → Renda (Income)
"SALARIO", "FOLHA"                      → Salário (Income)
"SUPERMERCADO", "MERCADO", "EXTRA"      → Alimentação (Expense)
"POSTO", "COMBUSTIVEL", "SHELL"         → Transporte
"NETFLIX", "SPOTIFY", "YOUTUBE"         → Assinaturas
"BOLETO"                                → Outras (Expense)
"INVESTIMENTO", "TESOURO"               → Investimentos
"BINANCE", "MERCADO BITCOIN"            → Investimentos (cripto)
default                                 → "Sem categoria"
```

## Configuração

```json
"Integrations": {
  "OpenFinance": {
    "BaseUrl": "https://matls-api.sandbox.directory.openbankingbrasil.org.br",
    "ClientId": "...",
    "ClientSecret": "...",
    "Mode": "Sandbox"   // ou "Mock" / "Production"
  }
}
```
