# Sprint 6 - Tasks

> Estimativa: **34 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S6-01 - Entidade IntegrationConfig (5 pts)
- [x] `IntegrationConfig : BaseEntity` (Provider enum, ApiKeyEncrypted, ApiSecretEncrypted, Active, LastSyncAt)
- [x] Service `ICryptoCipher` (AES) em Infrastructure (`AesCryptoCipher`)

## US-S6-02 - Cliente Binance (5 pts)
- [x] Wrapper `IBinanceClient` em Application
- [x] Implementação `BinanceClient : IBinanceClient` em Infrastructure
- [ ] Polly: retry 3x + circuit breaker — **não encontrado**

## US-S6-03 - Pipeline de Sync (8 pts)
- [x] `SyncBinanceCommand`
- [x] Pipeline em etapas: AccountInfo / Trades / Deposits / Withdrawals
- [x] Idempotência via `IntegrationSourceId`

## US-S6-04 - BinanceQuoteProvider (3 pts)
- [x] `BinanceQuoteProvider : IQuoteProvider` usando endpoint público `/api/v3/ticker/price`
- [x] Suporte a BTC, ETH, ADA, SOL, BNB, USDT, ...
- [x] Substituir o `MockQuoteProvider` na composição (registrado no DI)

## US-S6-05 - Endpoints (3 pts)
- [x] POST `/integracoes/binance` (config + test)
- [x] POST `/integracoes/binance/sync`
- [x] GET `/integracoes/binance/status`

## US-S6-06 - Migration (2 pts)
- [ ] `20260810_0000_Sprint06_Integrations` — **NÃO gerada**

## US-S6-07 - Testes (5 pts)
- [ ] Unit: handler de sync com mock do IBinanceClient
- [ ] Idempotência (rodar sync 2x não duplica operações)
- [ ] Integration: criptografia + persistência de chaves

## US-S6-08 - Documentação cripto (3 pts)
- [ ] Documentar mapeamento de pares (`BTCUSDT` → ticker `BTC` em USDT)
- [ ] Política de KYC / segurança de chaves
