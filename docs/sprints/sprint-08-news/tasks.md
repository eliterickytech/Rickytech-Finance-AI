# Sprint 8 - Tasks

> Estimativa: **21 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S8-01 - Entidade NewsItem (3 pts)
- [x] `NewsItem(Id, Title, Url, Source, Category, Tags[], PublishedAt, Summary, ImageUrl)`
- [x] Index único em `UrlHash` para dedupe

## US-S8-02 - INewsAggregator + Implementação (5 pts)
- [x] `RssNewsAggregator` parsing com `System.ServiceModel.Syndication`
- [x] Suporte a Atom + RSS 2.0

## US-S8-03 - BackgroundService (3 pts)
- [x] `NewsRefreshHostedService` (timer 15 min)
- [x] Dedupe concurrency-safe (verifica `UrlHash` antes de inserir)

## US-S8-04 - Auto-tagging (2 pts)
- [x] Função simples keyword → tag
- [ ] Lista configurável em `appsettings.json` — _verificar (tags podem estar hardcoded)_

## US-S8-05 - Endpoint + Query (3 pts)
- [x] `GetNewsQuery` com filtros + paginação
- [x] `NoticiasController : BaseApiController`

## US-S8-06 - Migration (2 pts)
- [ ] `20260907_0000_Sprint08_News` — **nenhuma migration gerada no projeto**

## US-S8-07 - Testes (3 pts)
- [ ] Parse de RSS fixtures (CoinDesk + InfoMoney)
- [ ] Dedupe por URL
- [ ] Cleanup TTL
