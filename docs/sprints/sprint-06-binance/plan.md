# Sprint 6 - Integração Binance

## Objetivo
Integrar com a **Binance** (maior exchange de cripto do mundo) para importar
saldos, trades, depósitos e saques do usuário e gerar automaticamente
Investimentos, Receitas (vendas) e Despesas (compras / taxas).

## Escopo IN
- `IntegrationConfig` entidade: armazena chaves API criptografadas (DPAPI / AES) por provedor.
- Lib: `Binance.Net` (NuGet oficial).
- Endpoints:
  - `POST /api/v1/integracoes/binance` (salvar chaves + testar conexão)
  - `POST /api/v1/integracoes/binance/sync` (importar dados)
  - `GET  /api/v1/integracoes/binance/status`
- Pipeline de ingestão:
  1. Buscar account info → atualizar saldos (cria/atualiza `Bank` cripto + `Investment` por ativo)
  2. Buscar `MyTrades` desde a última sync → criar `InvestmentOperation`
  3. Buscar `DepositHistory` → criar `Income`
  4. Buscar `WithdrawHistory` → criar `Expense`
- Idempotência via `IntegrationSourceId = "BINANCE:<orderId>"`.
- Implementar `BinanceQuoteProvider : IQuoteProvider` (real-time price para BTC, ETH, ADA, SOL, BNB, ...).
- Migration `20260810_0000_Sprint06_Integrations`.
- Tela no frontend fica no Sprint 11.

## Escopo OUT
- Futures/Margin/Options (apenas Spot neste sprint).
- Staking / Earn (sprint futuro).

## Decisões
- Chaves API com permissão somente leitura (validar no `POST` testando endpoint informativo).
- Sync incremental: armazenar `LastSyncAt` por endpoint.
- Rate limit Binance: usar `Polly` para retry exponential + circuit breaker.

## Riscos
| Risco                                       | Mitigação                                |
|---------------------------------------------|-------------------------------------------|
| Chaves vazadas                              | Criptografia at-rest + nunca expor no GET |
| Sync gigantesco no first run                | Cap em 90 dias + paginação                |
| Binance ban por IP                          | Backoff + reuso de proxy                  |

## Critério de pronto (DoD)
- Conectar conta de teste (testnet ou conta com saldo pequeno)
- Importar trades + saldos → criar `Investment` para BTC, ETH, ADA, SOL
- Atualizar cotações em tempo real via `BinanceQuoteProvider`
- Testes: integração com WireMock simulando responses Binance
