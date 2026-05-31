# Sprint 5 - Projeções de Lucros e Fluxo de Caixa

## Objetivo
Entregar o motor de **projeções** que combina receitas e despesas recorrentes
com simulação de juros compostos dos investimentos.

## Escopo IN
- Service `IProjectionService` em Application.
- Endpoint `POST /api/v1/projecoes` recebe horizonte (meses, default 12) e cenário (`Otimista`, `Realista`, `Pessimista`).
- Output: saldo projetado mês-a-mês por banco e consolidado, breakdown de receitas/despesas previstas.
- Cálculo de **juros compostos** para investimentos com `ExpectedYieldPercent` (anual).
- Sensibilidade a `InflationPercent` (parâmetro de cenário).
- Cache em memória (5 min) por chave (horizonte, cenário, hash dos lançamentos).

## Escopo OUT
- Análise Monte Carlo (futuro).
- Importar projeções de IA / ML (futuro).

## Decisões
- Cenários:
  - Otimista: investimentos rendem `expected × 1.20`, despesas `× 0.95`
  - Realista: rendem `expected`
  - Pessimista: rendem `expected × 0.80`, despesas `× 1.10`
- Período mínimo 1 mês, máximo 60.

## Critério de pronto (DoD)
- Endpoint retorna JSON com gráfico mensal de saldo.
- Testes parametrizados validam 3 cenários com inputs conhecidos.
- Performance: < 500ms para 36 meses + 50 lançamentos recorrentes.
