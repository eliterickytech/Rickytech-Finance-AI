# Sprint 8 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- Entidade `NewsItem` + abstração `INewsAggregator` com `RssNewsAggregator`.
- `NewsRefreshHostedService` (BackgroundService) atualizando o cache periodicamente.
- CQRS de leitura (`GetNewsQuery`) + DTOs + mapping.
- `NoticiasController` com filtros (categoria, tag, busca, paginação).
- ❌ Sem testes do agregador / auto-tagging.
- ⚠️ Migration ainda não gerada.

## Status das US

| US                                  | Status          |
|-------------------------------------|-----------------|
| US-S8-01 NewsItem                   | ✅ Implementado |
| US-S8-02 RssNewsAggregator          | ✅ Implementado |
| US-S8-03 BackgroundService          | ✅ Implementado |
| US-S8-04 Auto-tagging               | ✅ Implementado |
| US-S8-05 Endpoint + Query           | ✅ Implementado |
| US-S8-06 Migration                  | ❌ Pendente (não gerada) |
| US-S8-07 Testes                     | ❌ Pendente |

## Bloqueios
- **Migration do EF Core ausente** (ver Sprint 1).

## Demo prevista
- API retorna últimas 50 notícias de cripto com tags BTC/ETH/ADA/SOL
- Background job atualizando cache a cada 15 min visível em log
