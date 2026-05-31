# Sprint 4 - Tasks

> Estimativa: **29 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S4-01 - Entidades Investment + Operation (8 pts)
- [x] `Investment : BaseEntity` (Ticker, Type, Quantity, AveragePrice, AcquiredAt, BankId, Notes)
- [x] `InvestmentOperation : BaseEntity` (InvestmentId, Side: Buy/Sell, Quantity, Price, Fee, ExecutedAt)
- [x] Método `Investment.ApplyOperation(op)` recalcula AveragePrice

## US-S4-02 - AssetQuote + IQuoteProvider (5 pts)
- [x] Entidade `AssetQuote(Ticker, Date, Price, Currency, Source)`
- [x] `IQuoteProvider` em Application
- [x] Implementação mock em Infrastructure (`MockQuoteProvider`)

## US-S4-03 - Commands + Queries (5 pts)
- [x] CreateInvestment / RegisterOperation / DeleteInvestment
- [x] GetInvestmentById / GetInvestmentsByType / GetPortfolioSummary

## US-S4-04 - Validators (2 pts)
- [ ] Ticker obrigatório, formato esperado por tipo — **pasta `Validators/` vazia**
- [ ] Quantity > 0, AveragePrice >= 0

## US-S4-05 - Controller (3 pts)
- [x] `InvestimentosController : BaseApiController`
- [x] Endpoint extra `GET /resumo` (portfolio com lucro/prejuízo agregado)

## US-S4-06 - Migration (2 pts)
- [ ] `20260713_0000_Sprint04_Investments` — **NÃO gerada**

## US-S4-07 - Testes (4 pts)
- [x] Unit dos métodos `ApplyOperation` (média ponderada) — `InvestmentTests`
- [ ] Integration de criação + cotação mock + cálculo de lucro
