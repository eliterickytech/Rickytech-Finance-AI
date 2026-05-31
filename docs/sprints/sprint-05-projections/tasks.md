# Sprint 5 - Tasks

> Estimativa: **21 pts**

## US-S5-01 - IProjectionService (5 pts)
- [ ] Interface em Application
- [ ] Implementação em Infrastructure ou Application (puro)
- [ ] Função `Project(horizonMonths, scenario, snapshot)`

## US-S5-02 - Modelagem de cenário (3 pts)
- [ ] `ProjectionScenario { Optimistic, Realistic, Pessimistic }`
- [ ] `ScenarioParameters` (multiplicadores)

## US-S5-03 - Cálculo de juros compostos (3 pts)
- [ ] Função pura `CompoundInterest(principal, monthlyRate, months)`
- [ ] Cobrir Renda Fixa e Cripto (apenas se `ExpectedYieldPercent` definido)

## US-S5-04 - Command + Query (3 pts)
- [ ] `RunProjectionCommand` (parâmetros)
- [ ] Retorna `ProjectionResultDto`

## US-S5-05 - Controller (2 pts)
- [ ] `ProjecoesController : BaseApiController`

## US-S5-06 - Cache + Idempotência (2 pts)
- [ ] MemoryCache com chave hash de inputs

## US-S5-07 - Testes (3 pts)
- [ ] Casos parametrizados via `[Theory]`
- [ ] Cenário com 0 lançamentos retorna apenas OpeningBalance
