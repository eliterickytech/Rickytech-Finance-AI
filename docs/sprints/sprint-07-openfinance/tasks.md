# Sprint 7 - Tasks

> Estimativa: **31 pts**

## US-S7-01 - IOpenFinanceClient (5 pts)
- [ ] Interface em Application
- [ ] Implementação `OpenFinanceClient` em Infrastructure (HttpClient + Polly)

## US-S7-02 - Fluxo de Consentimento (8 pts)
- [ ] Generate consent → URL de redirect
- [ ] Persistir `consentId` em `IntegrationConfig.Metadata`
- [ ] Callback troca code por token

## US-S7-03 - Sync de transações (5 pts)
- [ ] `SyncOpenFinanceCommand`
- [ ] Paginar `/accounts/{accountId}/transactions`
- [ ] Idempotência via IntegrationSourceId

## US-S7-04 - Categorização automática (3 pts)
- [ ] `ITransactionClassifier` em Application
- [ ] Regras por keyword (Pix, Boleto, Supermercado, Combustível, Salário, ...)
- [ ] Fallback "Sem categoria"

## US-S7-05 - Endpoints (3 pts)
- [ ] POST consentir / GET callback / POST sync / GET status

## US-S7-06 - Migration (2 pts)
- [ ] `20260824_0000_Sprint07_OpenFinance` (apenas se houver novas tabelas; senão usar IntegrationConfig existente)

## US-S7-07 - Testes (5 pts)
- [ ] WireMock para sandbox
- [ ] Caso: 0 transações, N transações, paginação
- [ ] Caso de refresh token
