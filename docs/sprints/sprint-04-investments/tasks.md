# Sprint 4 - Tasks

> Estimativa: **29 pts**

## US-S4-01 - Entidades Investment + Operation (8 pts)
- [ ] `Investment : BaseEntity` (Ticker, Type, Quantity, AveragePrice, AcquiredAt, BankId, Notes)
- [ ] `InvestmentOperation : BaseEntity` (InvestmentId, Side: Buy/Sell, Quantity, Price, Fee, ExecutedAt)
- [ ] Método `Investment.ApplyOperation(op)` recalcula AveragePrice

## US-S4-02 - AssetQuote + IQuoteProvider (5 pts)
- [ ] Entidade `AssetQuote(Ticker, Date, Price, Currency, Source)`
- [ ] `IQuoteProvider` em Application
- [ ] Implementação mock em Infrastructure (`MockQuoteProvider`)

## US-S4-03 - Commands + Queries (5 pts)
- [ ] CreateInvestment / RegisterOperation / DeleteInvestment
- [ ] GetInvestmentById / GetInvestmentsByType / GetPortfolioSummary

## US-S4-04 - Validators (2 pts)
- [ ] Ticker obrigatório, formato esperado por tipo
- [ ] Quantity > 0, AveragePrice >= 0

## US-S4-05 - Controller (3 pts)
- [ ] `InvestimentosController : BaseApiController`
- [ ] Endpoint extra `GET /resumo` (portfolio com lucro/prejuízo agregado)

## US-S4-06 - Migration (2 pts)
- [ ] `20260713_0000_Sprint04_Investments`

## US-S4-07 - Testes (4 pts)
- [ ] Unit dos métodos `ApplyOperation` (média ponderada)
- [ ] Integration de criação + cotação mock + cálculo de lucro
