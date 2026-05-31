# Sprint 3 - Spec técnica (Receitas e Despesas)

## Hierarquia

```csharp
public abstract class FinancialTransaction : BaseEntity
{
    public string Description { get; protected set; }
    public decimal Amount { get; protected set; }
    public DateOnly Date { get; protected set; }
    public Guid CategoryId { get; protected set; }
    public Guid BankId { get; protected set; }
    public string[] Tags { get; protected set; } = Array.Empty<string>();
    public RecurrenceFrequency Recurrence { get; protected set; } = RecurrenceFrequency.Once;
    public DateOnly? RecurrenceEnd { get; protected set; }
    public Guid? IntegrationSourceId { get; protected set; }    // Sprint 6/7
    public string? AttachmentPath { get; protected set; }
    public bool Confirmed { get; protected set; }
}

public sealed class Income : FinancialTransaction { ... }

public sealed class Expense : FinancialTransaction
{
    public PaymentMethod PaymentMethod { get; private set; }
}
```

## Endpoints

| Verbo  | Rota                                  | Comando/Query              |
|--------|---------------------------------------|-----------------------------|
| POST   | `/api/v1/receitas`                    | `CreateIncomeCommand`       |
| GET    | `/api/v1/receitas`                    | `GetIncomesQuery`           |
| GET    | `/api/v1/receitas/{id}`               | `GetIncomeByIdQuery`        |
| PUT    | `/api/v1/receitas/{id}`               | `UpdateIncomeCommand`       |
| DELETE | `/api/v1/receitas/{id}`               | `DeleteIncomeCommand`       |
| POST   | `/api/v1/receitas/{id}/duplicar`      | `DuplicateIncomeCommand`    |
| POST   | `/api/v1/despesas`                    | `CreateExpenseCommand`      |
| ...    | `/api/v1/despesas/...`                | ...                         |
| POST   | `/api/v1/despesas/{id}/anexo`         | `UploadExpenseAttachmentCommand` |

## Filtros do GET

`startDate`, `endDate`, `categoryId`, `bankId`, `tags` (array), `minAmount`,
`maxAmount`, `recurrence`, `confirmed`, `page`, `pageSize` (default 50, max 200),
`sort` (default `Date desc`).

## Recorrência - regras

- Frequência | Próximas datas
- `Daily` | +1 dia
- `Weekly` | +7 dias
- `BiWeekly` | +14 dias
- `Monthly` | +1 mês (dia preservado, se dia inválido usa último dia do mês)
- `Quarterly` | +3 meses
- `SemiAnnual` | +6 meses
- `Annual` | +12 meses

Limite: até `RecurrenceEnd` ou +24 meses se nulo.

## Saldo atual de um Bank

```
CurrentBalance = OpeningBalance
               + Σ Income.Amount    (where BankId = X and Confirmed)
               - Σ Expense.Amount   (where BankId = X and Confirmed)
```

Implementar como `IQueryService` para evitar overhead em listagens. Cache de
5 minutos na camada Infrastructure.
