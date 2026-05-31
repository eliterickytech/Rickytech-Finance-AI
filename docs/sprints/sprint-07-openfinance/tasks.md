# Sprint 7 - Tasks

> Estimativa: **31 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S7-01 - IOpenFinanceClient (5 pts)
- [x] Interface em Application
- [x] Implementação `OpenFinanceClient` em Infrastructure (HttpClient) — _sem Polly_

## US-S7-02 - Fluxo de Consentimento (8 pts)
- [x] Generate consent → URL de redirect
- [x] Persistir `consentId` em `IntegrationConfig.Metadata`
- [x] Callback troca code por token

## US-S7-03 - Sync de transações (5 pts)
- [x] `SyncOpenFinanceCommand`
- [x] Paginar `/accounts/{accountId}/transactions`
- [x] Idempotência via `IntegrationSourceId`

## US-S7-04 - Categorização automática (3 pts)
- [ ] `ITransactionClassifier` em Application — **categorização está inline em `OpenFinanceCommands`, sem interface dedicada**
- [x] Regras por keyword (Pix, Boleto, Supermercado, Combustível, Salário, ...)
- [x] Fallback "Sem categoria"

## US-S7-05 - Endpoints (3 pts)
- [x] POST consentir / GET callback / POST sync / GET status

## US-S7-06 - Migration (2 pts)
- [ ] `20260824_0000_Sprint07_OpenFinance` — **nenhuma migration gerada no projeto**

## US-S7-07 - Testes (5 pts)
- [ ] WireMock para sandbox
- [ ] Caso: 0 transações, N transações, paginação
- [ ] Caso de refresh token
