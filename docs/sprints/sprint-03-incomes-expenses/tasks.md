# Sprint 3 - Tasks

> Estimativa: **42 pts**

## US-S3-01 - Base FinancialTransaction (5 pts)
- [ ] Classe abstrata `FinancialTransaction : BaseEntity` com campos comuns
- [ ] Strategy de discriminação (TPH) para Income/Expense

## US-S3-02 - Entidades Income / Expense (5 pts)
- [ ] `Income : FinancialTransaction`
- [ ] `Expense : FinancialTransaction` com `PaymentMethod`
- [ ] Relacionamentos com Category e Bank

## US-S3-03 - Recorrência (8 pts)
- [ ] Enum `RecurrenceFrequency`
- [ ] Service `IRecurrenceProjector` em Application
- [ ] Comando `GenerateUpcomingTransactionsCommand` (gera próximos N)

## US-S3-04 - Commands + Queries Income (5 pts)
- [ ] CreateIncome / UpdateIncome / DeleteIncome / DuplicateIncome
- [ ] GetIncomeById / GetIncomes (com filtros)

## US-S3-05 - Commands + Queries Expense (5 pts)
- [ ] Idem para despesas + endpoint de anexo

## US-S3-06 - Controllers (3 pts)
- [ ] `ReceitasController` + `DespesasController`

## US-S3-07 - Recalcular saldo (5 pts)
- [ ] Método `Bank.CalculateCurrentBalance(transactions)` ou QueryService
- [ ] Index em `BankId + Date` para performance

## US-S3-08 - Migration (2 pts)
- [ ] `20260629_0000_Sprint03_IncomesExpenses`

## US-S3-09 - Testes (4 pts)
- [ ] Unit + Integration + API tests do happy path e edge cases
