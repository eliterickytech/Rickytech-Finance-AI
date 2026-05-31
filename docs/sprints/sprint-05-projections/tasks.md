# Sprint 5 - Tasks

> Estimativa: **21 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S5-01 - IProjectionService (5 pts)
- [ ] Interface em Application — **não existe (`IProjectionService` ausente)**
- [ ] Implementação em Infrastructure ou Application (puro) — _lógica vive inline no `RunProjectionCommand`_
- [ ] Função `Project(horizonMonths, scenario, snapshot)`

## US-S5-02 - Modelagem de cenário (3 pts)
- [x] `ProjectionScenario { Optimistic, Realistic, Pessimistic }`
- [x] `ScenarioParameters` (multiplicadores) — via `GetMultipliers(scenario)`

## US-S5-03 - Cálculo de juros compostos (3 pts)
- [ ] Função pura `CompoundInterest(principal, monthlyRate, months)` — _lógica inline no handler (`Math.Pow`), sem função isolada_
- [x] Cobrir Renda Fixa e Cripto (yield mensal aplicado por cenário)

## US-S5-04 - Command + Query (3 pts)
- [x] `RunProjectionCommand` (parâmetros)
- [x] Retorna `ProjectionResultDto`

## US-S5-05 - Controller (2 pts)
- [x] `ProjecoesController : BaseApiController`

## US-S5-06 - Cache + Idempotência (2 pts)
- [ ] MemoryCache com chave hash de inputs — **não implementado**

## US-S5-07 - Testes (3 pts)
- [ ] Casos parametrizados via `[Theory]`
- [ ] Cenário com 0 lançamentos retorna apenas OpeningBalance
