# Sprint 4 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- Entidades `Investment` e `InvestmentOperation` + enum `InvestmentType` implementadas.
- `AssetQuote` + abstração `IQuoteProvider` com `MockQuoteProvider` para cotações.
- CQRS (`CreateInvestmentCommand`, `InvestmentQueries`), mappings e `InvestimentosController`.
- Testes de domínio presentes (`InvestmentTests`).
- ❌ Pasta `Validators/` de Investments está **vazia** — falta FluentValidation para os commands.
- ⚠️ Migration ainda não gerada.

## Status das US

| US                                       | Status          |
|------------------------------------------|-----------------|
| US-S4-01 Investment + Operation          | ✅ Implementado |
| US-S4-02 AssetQuote + IQuoteProvider     | ✅ Implementado |
| US-S4-03 Commands + Queries              | ✅ Implementado |
| US-S4-04 Validators                      | ❌ Pendente (pasta vazia) |
| US-S4-05 Controller                      | ✅ Implementado |
| US-S4-06 Migration                       | ❌ Pendente (não gerada) |
| US-S4-07 Testes                          | 🟡 Parcial (só `InvestmentTests` de domínio; falta integração) |

## Bloqueios
- **Migration do EF Core ausente** (ver Sprint 1).

## Demo prevista
- Cadastrar posições em BTC, ETH, ADA, SOL
- Mostrar lucro/prejuízo com cotação mockada
- Endpoint `/api/v1/investimentos/resumo` retornando agregação por tipo
