# Sprint 3 - CRUD Receitas e Despesas

## Objetivo
Entregar os dois CRUDs mais importantes do sistema, com suporte a **recorrência**
e **tags**, base para todo o cálculo financeiro e projeções.

## Escopo IN
- Entidades `Income` e `Expense` (espelhadas) compartilhando `FinancialTransaction` (base abstrata).
- Enums:
  - `RecurrenceFrequency { Once, Daily, Weekly, BiWeekly, Monthly, Quarterly, SemiAnnual, Annual }`
  - `PaymentMethod { Cash, Debit, Credit, Pix, Boleto, TransferBank, Crypto }`
- Geração automática de lançamentos futuros para entradas recorrentes (job manual em dev; CronJob no Sprint 12).
- CRUD MediatR para ambos.
- Filtros: período (dataInicio/dataFim), categoria, banco, tags, recorrente, faixa de valor.
- Endpoints específicos:
  - `POST /receitas/{id}/duplicar` (clonar lançamento)
  - `POST /despesas/{id}/anexo` (upload de comprovante)
- Migration `20260629_0000_Sprint03_IncomesExpenses`.
- Recalcular saldo atual do Banco a partir dos lançamentos (view ou método).

## Escopo OUT
- Categorização automática por NLP (futuro).
- Frontend (Sprint 10).

## Decisões
- Cada `Income`/`Expense` é imutável após confirmado (>= D+1); edição cria uma reversão + novo lançamento.
- Anexo: caminho local em `wwwroot/uploads/` (Sprint 3); migrar para Azure Blob no Sprint 12.

## Critério de pronto (DoD)
- CRUD funcional via Swagger.
- Saldo atual do banco reflete soma de receitas - despesas + OpeningBalance.
- Recorrência projeta corretamente os próximos 12 lançamentos.
- Testes >= 80%.
