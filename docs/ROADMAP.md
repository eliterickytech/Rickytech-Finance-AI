# Roadmap - Finance Control

Projeto dividido em **13 sprints** (Sprint 0 a Sprint 12). Sprints curtos
(2 semanas cada), com entrega incremental.

| Sprint | Objetivo                                  | Backend | Frontend | Duração |
|--------|-------------------------------------------|:-------:|:--------:|:-------:|
| **00** | Setup do projeto, solução, arquitetura, CI/CD, Serilog, Swagger | ● |    | 2 sem  |
| **01** | Domain base + CRUD **Categorias** (Receita/Despesa) | ● |    | 2 sem  |
| **02** | CRUD **Bancos** + saldos                 | ●       |          | 2 sem  |
| **03** | CRUD **Receitas** + CRUD **Despesas** (com recorrência) | ● |    | 2 sem  |
| **04** | CRUD **Investimentos** (Renda Fixa, Variável, Cripto) | ● |    | 2 sem  |
| **05** | **Projeções de lucros** e fluxo de caixa futuro | ● |          | 2 sem  |
| **06** | Integração **Binance** (BTC, ETH, ADA, SOL, ...) | ● |          | 2 sem  |
| **07** | Integração **OpenFinance** / bancos      | ●       |          | 2 sem  |
| **08** | Painel de **notícias** (cripto + financeiro) | ●   |          | 2 sem  |
| **09** | Frontend: setup React + template **Color Admin** |  | ●     | 2 sem  |
| **10** | Frontend: CRUDs + dashboard              |          | ●        | 2 sem  |
| **11** | Frontend: integrações + painel de notícias |        | ●        | 2 sem  |
| **12** | Testes (xUnit / Vitest), hardening, deploy | ●      | ●        | 2 sem  |

## Status atual (snapshot 2026-05-31)

> Auditoria feita a partir do **código-fonte** (os `progress.md` haviam ficado
> desatualizados). Legenda: ✅ Concluído · 🟡 Parcial · ❌ Pendente.

| Sprint | Tema | Status |
|--------|------|:------:|
| 00 | Setup / arquitetura / CI | ✅ |
| 01 | CRUD Categorias | ✅ (falta migration) |
| 02 | CRUD Bancos | ✅ (falta migration) |
| 03 | Receitas / Despesas / recorrência | ✅ (testes parciais) |
| 04 | Investimentos | 🟡 (sem validators) |
| 05 | Projeções | 🟡 (sem service dedicado/cache/testes) |
| 06 | Binance | ✅ (sem testes) |
| 07 | OpenFinance | ✅ (categorização a validar) |
| 08 | Notícias | ✅ (sem testes) |
| 09 | Frontend setup | ✅ (sem testes Vitest) |
| 10 | Frontend CRUDs | ✅ |
| 11 | Frontend integrações | ✅ (sem testes) |
| 12 | Testes / deploy | 🟡 |

### 🔴 Bloqueio transversal
**Migrations do EF Core não foram geradas** (`backend/src/FinanceControl.Data/Migrations/`
só tem `.gitkeep`). DbContext, Configurations e Seeder existem, mas sem migration
o banco não sobe. É o pré-requisito nº 1 para rodar a aplicação.

### Lacunas principais
- Migration inicial do EF Core (bloqueante).
- Cobertura de testes baixa (4 testes de domínio; sem Application/integração/E2E/Vitest).
- Auth sem endpoint de **registro** no backend (frontend já tem a tela).
- Validators ausentes em Investments e Projections.
- Limpeza: pasta literal `frontend/src/store/{slices}` criada por engano.

## Marcos (milestones)

- **M1 - Backend MVP funcional** → fim do Sprint 5 (CRUDs + Projeções)
- **M2 - Integrações vivas** → fim do Sprint 8 (Binance + OpenFinance + News)
- **M3 - Produto navegável** → fim do Sprint 11 (frontend completo)
- **M4 - Release 1.0** → fim do Sprint 12 (testes + deploy)

## Dependências críticas

- **Sprint 1** desbloqueia 2, 3, 4 (todos consomem Domain + DbContext).
- **Sprint 3** desbloqueia 5 (projeções precisam de receitas/despesas).
- **Sprint 6** depende do CRUD de Investimentos (Sprint 4) — receitas/saldos cripto entram via Binance.
- **Sprint 9** desbloqueia 10 e 11 (frontend).
- **Sprint 12** consolida tudo.

## Estrutura de cada pasta `docs/sprints/sprint-XX/`

```
plan.md       # Objetivos, escopo IN/OUT, decisões, riscos
tasks.md      # Tarefas (US-### com critérios de aceite e estimativa)
progress.md   # Diário: o que andou, o que travou, demos
spec.md       # Especificação técnica (endpoints, payloads, entidades, regras)
```
