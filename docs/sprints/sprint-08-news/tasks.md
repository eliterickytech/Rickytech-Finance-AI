# Sprint 8 - Tasks

> Estimativa: **21 pts**

## US-S8-01 - Entidade NewsItem (3 pts)
- [ ] `NewsItem(Id, Title, Url, Source, Category, Tags[], PublishedAt, Summary, ImageUrl)`
- [ ] Index único em `UrlHash` para dedupe

## US-S8-02 - INewsAggregator + Implementação (5 pts)
- [ ] `RssNewsAggregator` parsing com `System.ServiceModel.Syndication`
- [ ] Suporte a Atom + RSS 2.0

## US-S8-03 - BackgroundService (3 pts)
- [ ] `NewsRefreshHostedService` (timer 15 min)
- [ ] Concurrency-safe (lock por feed)

## US-S8-04 - Auto-tagging (2 pts)
- [ ] Função simples keyword → tag
- [ ] Lista configurável em `appsettings.json`

## US-S8-05 - Endpoint + Query (3 pts)
- [ ] `GetNewsQuery` com filtros + paginação
- [ ] `NoticiasController : BaseApiController`

## US-S8-06 - Migration (2 pts)
- [ ] `20260907_0000_Sprint08_News`

## US-S8-07 - Testes (3 pts)
- [ ] Parse de RSS fixtures (CoinDesk + InfoMoney)
- [ ] Dedupe por URL
- [ ] Cleanup TTL
