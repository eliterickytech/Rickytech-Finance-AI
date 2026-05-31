# Sprint 3 - Tasks

> Estimativa: **42 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S3-01 - Base FinancialTransaction (5 pts)
- [x] Classe abstrata `FinancialTransaction : BaseEntity` com campos comuns
- [x] Strategy de discriminação (TPH) para Income/Expense

## US-S3-02 - Entidades Income / Expense (5 pts)
- [x] `Income : FinancialTransaction`
- [x] `Expense : FinancialTransaction` com `PaymentMethod`
- [x] Relacionamentos com Category e Bank

## US-S3-03 - Recorrência (8 pts)
- [x] Enum `RecurrenceFrequency`
- [x] Service `IRecurrenceProjector` em Application (impl. `RecurrenceProjector`)
- [ ] Comando `GenerateUpcomingTransactionsCommand` (gera próximos N) — **não implementado**

## US-S3-04 - Commands + Queries Income (5 pts)
- [x] CreateIncome / UpdateIncome / DeleteIncome — _falta `DuplicateIncome`_
- [ ] GetIncomeById / GetIncomes — **só `GetIncomes` (lista) existe**

## US-S3-05 - Commands + Queries Expense (5 pts)
- [x] CRUD de despesas (`CrudExpenseCommands`, `GetExpensesQueries`) — _falta endpoint de anexo (upload)_

## US-S3-06 - Controllers (3 pts)
- [x] `ReceitasController` + `DespesasController`

## US-S3-07 - Recalcular saldo (5 pts)
- [ ] Método `Bank.CalculateCurrentBalance(transactions)` ou QueryService — **não encontrado**
- [ ] Index em `BankId + Date` para performance

## US-S3-08 - Migration (2 pts)
- [ ] `20260629_0000_Sprint03_IncomesExpenses` — **NÃO gerada**

## US-S3-09 - Testes (4 pts)
- [ ] Unit + Integration + API tests do happy path e edge cases — **só existe `RecurrenceProjectorTests`** (unit de domínio)
