# Sprint 5 - Spec técnica (Projeções)

## Endpoint

```
POST /api/v1/projecoes
Content-Type: application/json
{
  "horizonMonths": 12,
  "scenario": "Realistic",
  "inflationPercent": 4.5,
  "includeInvestments": true,
  "bankIds": null            // null = todos
}
```

## Response

```json
{
  "horizonMonths": 12,
  "scenario": "Realistic",
  "generatedAt": "2026-08-01T00:00:00Z",
  "series": [
    {
      "month": "2026-09",
      "openingBalance": 10000.00,
      "income": 12000.00,
      "expense": 9500.00,
      "investmentYield": 350.00,
      "endingBalance": 12850.00
    }
    // ... 12 itens
  ],
  "summary": {
    "endingBalance": 25000.00,
    "totalIncome": 144000.00,
    "totalExpense": 114000.00,
    "totalInvestmentYield": 4200.00,
    "netProfit": 34200.00
  }
}
```

## Algoritmo

```
para cada mês m em [1..horizonMonths]:
    1. projetar receitas recorrentes ativas em m (aplicar Frequency)
    2. projetar despesas recorrentes ativas em m
    3. para cada Investment com ExpectedYieldPercent:
         yield = principal * (ExpectedYieldPercent/12)
         aplicar multiplicador do cenário
       somar yield em investmentYield[m]
    4. endingBalance[m] = openingBalance[m] + income[m] - expense[m] + yield[m]
    5. openingBalance[m+1] = endingBalance[m]
```

## Cenários (multiplicadores)

| Cenário     | Income | Expense | Yield |
|-------------|:------:|:-------:|:-----:|
| Optimistic  | 1.05   | 0.95    | 1.20  |
| Realistic   | 1.00   | 1.00    | 1.00  |
| Pessimistic | 0.95   | 1.10    | 0.80  |

Aplicar também `(1 + inflationPercent/12/100)` mês a mês sobre despesas.

## Cache

Chave: `proj:{horizonMonths}:{scenario}:{inflationPercent}:{hash(snapshot)}`
TTL: 5 minutos. Invalidar em qualquer mudança nas tabelas Income/Expense/Investment.
