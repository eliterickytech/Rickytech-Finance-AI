# Sprint 0 - Tasks

> Estimativa total: **34 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S0-01 - Solution e projetos (5 pts)
- [x] Criar `FinanceControl.sln`
- [x] Criar 6 projetos `.csproj` (Api, Application, Domain, CrossCutting, Data, Infrastructure)
- [ ] Criar 3 projetos de teste (UnitTests, IntegrationTests, ApiTests) — **só `UnitTests` existe**
- [x] Configurar `Directory.Build.props` com net10.0, nullable, warnings-as-errors
- [x] Configurar referências entre projetos conforme `ARCHITECTURE.md`

**AC:** `dotnet build` da solução roda sem warnings/errors.

## US-S0-02 - Pacotes NuGet base (3 pts)
- [x] MediatR 12, AutoMapper 13, FluentValidation 11 (Application)
- [x] EF Core 10 (SqlServer + Sqlite) (Data)
- [x] Serilog + sinks (Api + CrossCutting)
- [x] Binance.Net + CoinGecko.Api (Infrastructure - apenas referenciados, integração no Sprint 6)

**AC:** `dotnet restore` sem erros.

## US-S0-03 - BaseApiController + ApiResponse (3 pts)
- [x] Criar `ApiResponse<T>` no `Application/Common/Models`
- [x] Criar `BaseApiController` no `Api/Controllers`
- [x] Implementar helpers: `OkResponse`, `CreatedResponse`, `NoContentResponse`, `BadRequestResponse`, `NotFoundResponse`

**AC:** Controller dummy retorna envelope padronizado.

## US-S0-04 - Middlewares (5 pts)
- [x] `ExceptionHandlingMiddleware` mapeando `ValidationException` / `NotFoundException` / `UnauthorizedException`
- [x] `CorrelationIdMiddleware` (lê/gera `X-Correlation-Id` e injeta no `LogContext`)
- [x] Registrar no pipeline do `Program.cs` na ordem correta

**AC:** Throwing manual de `NotFoundException` retorna 404 com envelope; header `X-Correlation-Id` presente.

## US-S0-05 - Serilog fortemente tipado (3 pts)
- [x] Configurar via `appsettings.json` (`Serilog:WriteTo`, `Serilog:MinimumLevel`, enrichers)
- [x] Sinks: Console + File rolling diário
- [x] Enrichers: FromLogContext, WithMachineName, WithThreadId
- [x] Adicionar `UseSerilogRequestLogging`

**AC:** Cada request gera log estruturado com path, status, elapsed e CorrelationId.

## US-S0-06 - CrossCutting DI (3 pts)
- [x] `AddApplicationDependencies` (MediatR, AutoMapper, FluentValidation, behaviors)
- [x] `AddInfrastructureDependencies` (placeholders para DbContext, HttpClients)
- [x] `AddApiDependencies` (Controllers, Swagger, CORS, HealthChecks)

**AC:** `Program.cs` registra tudo via os 3 helpers, sem strings mágicas.

## US-S0-07 - Pipeline behaviors do MediatR (3 pts)
- [x] `ValidationBehavior<TRequest, TResponse>`
- [x] `LoggingBehavior<TRequest, TResponse>`
- [x] Registrar em `AddApplicationDependencies`

**AC:** Command sem validator passa direto; com validator inválido lança `ValidationException`.

## US-S0-08 - Swagger + CORS + HealthCheck (2 pts)
- [x] Swagger habilitado em dev
- [x] CORS policy `FinanceControlPolicy` (origens localhost:3000 e 5173)
- [x] HealthChecks em `/health` (sem dependências externas)

**AC:** Swagger UI lista (no mínimo) o health-check; `/health` retorna 200.

## US-S0-09 - Interfaces Application (2 pts)
- [x] `IApplicationDbContext` (com `SaveChangesAsync`)
- [x] `ICurrentUserService`
- [x] `IDateTime` (abstração de `DateTime.UtcNow` para testes)

**AC:** Interfaces compilam, sem implementações concretas (vêm no Sprint 1).

## US-S0-10 - CI/CD básico (3 pts)
- [x] Workflow `ci.yml`: build + test em PR para `main`
- [ ] Cache de NuGet — **não configurado no `ci.yml`** (só cache npm no frontend)
- [ ] Badge no README — **ausente**

**AC:** PR de exemplo dispara o workflow e fica verde.

## US-S0-11 - Documentação SDD do sprint (2 pts)
- [x] Preencher `progress.md` ao fim do sprint com o que foi entregue
- [x] Atualizar `ROADMAP.md` marcando Sprint 0 como concluído
