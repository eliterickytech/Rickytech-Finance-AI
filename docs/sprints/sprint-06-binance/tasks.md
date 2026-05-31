# Sprint 6 - Tasks

> Estimativa: **34 pts**

## US-S6-01 - Entidade IntegrationConfig (5 pts)
- [ ] `IntegrationConfig : BaseEntity` (Provider enum, ApiKeyEncrypted, ApiSecretEncrypted, Active, LastSyncAt)
- [ ] Service `ICryptoCipher` (AES-256-GCM) em Infrastructure

## US-S6-02 - Cliente Binance (5 pts)
- [ ] Wrapper `IBinanceClient` em Application
- [ ] Implementação `BinanceClient : IBinanceClient` em Infrastructure usando Binance.Net
- [ ] Polly: retry 3x + circuit breaker

## US-S6-03 - Pipeline de Sync (8 pts)
- [ ] `SyncBinanceCommand`
- [ ] Pipeline em 4 etapas: AccountInfo / Trades / Deposits / Withdrawals
- [ ] Idempotência via IntegrationSourceId

## US-S6-04 - BinanceQuoteProvider (3 pts)
- [ ] `BinanceQuoteProvider : IQuoteProvider` usando endpoint público `/api/v3/ticker/price`
- [ ] Suporte a BTC, ETH, ADA, SOL, BNB, USDT, MATIC, DOT, AVAX, DOGE
- [ ] Substituir o `MockQuoteProvider` na composição

## US-S6-05 - Endpoints (3 pts)
- [ ] POST `/integracoes/binance` (config + test)
- [ ] POST `/integracoes/binance/sync`
- [ ] GET `/integracoes/binance/status`

## US-S6-06 - Migration (2 pts)
- [ ] `20260810_0000_Sprint06_Integrations`

## US-S6-07 - Testes (5 pts)
- [ ] Unit: handler de sync com mock do IBinanceClient
- [ ] Idempotência (rodar sync 2x não duplica operações)
- [ ] Integration: criptografia + persistência de chaves

## US-S6-08 - Documentação cripto (3 pts)
- [ ] Documentar mapeamento de pares (`BTCUSDT` → ticker `BTC` em USDT)
- [ ] Política de KYC / segurança de chaves
