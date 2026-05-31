# Sprint 3 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- Base `FinancialTransaction` + `Income`/`Expense` implementadas, com `RecurrenceFrequency` e `PaymentMethod`.
- CQRS de Receitas (Create/Update/Delete + GetIncomes) e Despesas (`CrudExpenseCommands`, `GetExpensesQueries`), com mappings e validators.
- Controllers `ReceitasController` e `DespesasController`.
- `FinancialTransactionConfiguration` no EF.
- Recorrência coberta por `RecurrenceProjectorTests`.
- 🟡 Recálculo de saldo do banco: lógica presente no domínio/commands, mas **sem teste dedicado** confirmando o comportamento end-to-end.
- ⚠️ Migration ainda não gerada.

## Status das US

| US                                       | Status          |
|------------------------------------------|-----------------|
| US-S3-01 Base FinancialTransaction       | ✅ Implementado |
| US-S3-02 Income / Expense                | ✅ Implementado |
| US-S3-03 Recorrência                     | 🟡 Parcial (projector ok; falta `GenerateUpcomingTransactionsCommand`) |
| US-S3-04 Commands/Queries Income         | 🟡 Parcial (falta `DuplicateIncome` e `GetIncomeById`) |
| US-S3-05 Commands/Queries Expense        | 🟡 Parcial (falta endpoint de anexo) |
| US-S3-06 Controllers                     | ✅ Implementado |
| US-S3-07 Recalcular saldo                | 🟡 Parcial (sem teste dedicado) |
| US-S3-08 Migration                       | ❌ Pendente (não gerada) |
| US-S3-09 Testes                          | 🟡 Parcial (só recorrência; faltam handlers) |

## Bloqueios
- **Migration do EF Core ausente** (ver Sprint 1).

## Demo prevista
- Cadastrar receita recorrente mensal e ver 12 próximos lançamentos
- Cadastrar despesa avulsa com anexo
- Saldo do banco recalculado em tempo real
