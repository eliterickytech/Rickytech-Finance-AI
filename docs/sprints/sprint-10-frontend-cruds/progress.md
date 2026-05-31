# Sprint 10 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- RTK Query configurado (`apiSlice` + `httpClient`) com 9 APIs: banks, categories, integrations, investments, news, projections, transactions.
- Páginas implementadas e ligadas ao backend: `Dashboard`, `Categories`, `Banks`, `Incomes`, `Expenses`, `Investments`, `Projections` (+ `Settings`, `Auth/Login`, `Auth/Register`).
- Estado via RTK Query (pasta `store/slices` vazia — intencional).
- 🟢 Observação: existe uma pasta criada por engano `store/{slices}` (literal) que deve ser removida.

## Status das US

| US                              | Status          |
|---------------------------------|-----------------|
| US-S10-01 RTK Query             | ✅ Implementado |
| US-S10-02 Dashboard             | ✅ Implementado |
| US-S10-03 CRUD Categorias       | ✅ Implementado |
| US-S10-04 CRUD Bancos           | ✅ Implementado |
| US-S10-05 CRUD Receitas         | ✅ Implementado |
| US-S10-06 CRUD Despesas         | ✅ Implementado |
| US-S10-07 Tela Investimentos    | ✅ Implementado |
| US-S10-08 Tela Projeções        | ✅ Implementado |

## Bloqueios
- Depende do backend com **migration aplicada** para exibir dados reais (ver Sprint 1).

## Limpeza pendente
- Remover a pasta literal `frontend/src/store/{slices}` criada por engano.

## Demo prevista
- Dashboard com dados reais do backend
- Todos os CRUDs operáveis
- Projeção 12 meses com gráfico
