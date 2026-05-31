# Changelog

## v1.0.0 - 2026-05-25 (Sprint 12 - Release inicial)

### Backend (.NET 10)
- Clean Architecture com 6 projetos (Api, Application, Domain, CrossCutting, Data, Infrastructure)
- CQRS via MediatR com `ValidationBehavior` + `LoggingBehavior`
- `BaseApiController` padronizando envelope `ApiResponse<T>`
- Middlewares: `ExceptionHandlingMiddleware`, `CorrelationIdMiddleware`
- Serilog fortemente tipado (Console + File + Seq)
- EF Core 10 com providers SqlServer e Sqlite
- Migrations + Seed automatizado de categorias e bancos default

### Features
- CRUD Categorias (Receita / Despesa / Ambos)
- CRUD Bancos multi-moeda (BRL, USD, USDT, BTC, ETH, ADA, SOL, ...)
- CRUD Receitas com recorrência
- CRUD Despesas com forma de pagamento e recorrência
- CRUD Investimentos com cálculo de média ponderada e cotação em tempo real
- Endpoint de portfólio com lucro/prejuízo agregado
- Projeções de fluxo de caixa (cenários otimista/realista/pessimista, juros compostos)
- Integração Binance (chaves AES-256 criptografadas, sync de saldos/trades/depósitos/saques)
- `BinanceQuoteProvider` para cotações cripto em tempo real
- Integração Open Finance Brasil (consent + callback + sync + categorização por keyword)
- Painel de notícias agregando RSS (CoinDesk, CoinTelegraph, InfoMoney) com auto-tagging
- Background service que atualiza notícias a cada 15 min
- Auth JWT (Bearer) com Swagger integrado
- Rate limit FixedWindow (100 req/min global, 10 req/min em /auth)

### Frontend (React 18 + TS + Vite)
- Layout inspirado no Color Admin (sidebar dark fixa + header dark)
- 11 páginas: Dashboard, Categorias, Bancos, Receitas, Despesas, Investimentos,
  Projeções, Integração Binance, Integração OpenFinance, Notícias, Configurações
- Tema dark/light com persistência
- i18n PT-BR / EN-US
- RTK Query (estado de servidor + cache + invalidação de tags)

### DevOps
- Dockerfile multi-stage para API
- Dockerfile + nginx para frontend
- `docker-compose.yml` (API + Frontend + SQL Server + Seq)
- GitHub Actions CI (build + test backend, build frontend)
- Testes unitários xUnit + FluentAssertions (Domain, RecurrenceProjector)

### Documentação SDD
- README, ARCHITECTURE, ROADMAP, SPEC globais
- 13 sprints com plan/tasks/progress/spec
- Runbook operacional
