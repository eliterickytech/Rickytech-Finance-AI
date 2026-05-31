# Sprint 0 - Tasks

> Estimativa total: **34 pts**

## US-S0-01 - Solution e projetos (5 pts)
- [ ] Criar `FinanceControl.sln`
- [ ] Criar 6 projetos `.csproj` (Api, Application, Domain, CrossCutting, Data, Infrastructure)
- [ ] Criar 3 projetos de teste (UnitTests, IntegrationTests, ApiTests)
- [ ] Configurar `Directory.Build.props` com net10.0, nullable, warnings-as-errors
- [ ] Configurar referências entre projetos conforme `ARCHITECTURE.md`

**AC:** `dotnet build` da solução roda sem warnings/errors.

## US-S0-02 - Pacotes NuGet base (3 pts)
- [ ] MediatR 12, AutoMapper 13, FluentValidation 11 (Application)
- [ ] EF Core 10 (SqlServer + Sqlite) (Data)
- [ ] Serilog + sinks (Api + CrossCutting)
- [ ] Binance.Net + CoinGecko.Api (Infrastructure - apenas referenciados, integração no Sprint 6)

**AC:** `dotnet restore` sem erros.

## US-S0-03 - BaseApiController + ApiResponse (3 pts)
- [ ] Criar `ApiResponse<T>` no `Application/Common/Models`
- [ ] Criar `BaseApiController` no `Api/Controllers`
- [ ] Implementar helpers: `OkResponse`, `CreatedResponse`, `NoContentResponse`, `BadRequestResponse`, `NotFoundResponse`

**AC:** Controller dummy retorna envelope padronizado.

## US-S0-04 - Middlewares (5 pts)
- [ ] `ExceptionHandlingMiddleware` mapeando `ValidationException` / `NotFoundException` / `UnauthorizedException`
- [ ] `CorrelationIdMiddleware` (lê/gera `X-Correlation-Id` e injeta no `LogContext`)
- [ ] Registrar no pipeline do `Program.cs` na ordem correta

**AC:** Throwing manual de `NotFoundException` retorna 404 com envelope; header `X-Correlation-Id` presente.

## US-S0-05 - Serilog fortemente tipado (3 pts)
- [ ] Configurar via `appsettings.json` (`Serilog:WriteTo`, `Serilog:MinimumLevel`, enrichers)
- [ ] Sinks: Console + File rolling diário
- [ ] Enrichers: FromLogContext, WithMachineName, WithThreadId
- [ ] Adicionar `UseSerilogRequestLogging`

**AC:** Cada request gera log estruturado com path, status, elapsed e CorrelationId.

## US-S0-06 - CrossCutting DI (3 pts)
- [ ] `AddApplicationDependencies` (MediatR, AutoMapper, FluentValidation, behaviors)
- [ ] `AddInfrastructureDependencies` (placeholders para DbContext, HttpClients)
- [ ] `AddApiDependencies` (Controllers, Swagger, CORS, HealthChecks)

**AC:** `Program.cs` registra tudo via os 3 helpers, sem strings mágicas.

## US-S0-07 - Pipeline behaviors do MediatR (3 pts)
- [ ] `ValidationBehavior<TRequest, TResponse>`
- [ ] `LoggingBehavior<TRequest, TResponse>`
- [ ] Registrar em `AddApplicationDependencies`

**AC:** Command sem validator passa direto; com validator inválido lança `ValidationException`.

## US-S0-08 - Swagger + CORS + HealthCheck (2 pts)
- [ ] Swagger habilitado em dev
- [ ] CORS policy `FinanceControlPolicy` (origens localhost:3000 e 5173)
- [ ] HealthChecks em `/health` (sem dependências externas)

**AC:** Swagger UI lista (no mínimo) o health-check; `/health` retorna 200.

## US-S0-09 - Interfaces Application (2 pts)
- [ ] `IApplicationDbContext` (com `SaveChangesAsync`)
- [ ] `ICurrentUserService`
- [ ] `IDateTime` (abstração de `DateTime.UtcNow` para testes)

**AC:** Interfaces compilam, sem implementações concretas (vêm no Sprint 1).

## US-S0-10 - CI/CD básico (3 pts)
- [ ] Workflow `ci.yml`: build + test em PR para `main`
- [ ] Cache de NuGet
- [ ] Badge no README

**AC:** PR de exemplo dispara o workflow e fica verde.

## US-S0-11 - Documentação SDD do sprint (2 pts)
- [ ] Preencher `progress.md` ao fim do sprint com o que foi entregue
- [ ] Atualizar `ROADMAP.md` marcando Sprint 0 como concluído
