# Arquitetura - Finance Control

## 1. Visão geral

O backend segue **Clean Architecture** + **CQRS** com cinco camadas físicas
(além de Infrastructure e dos projetos de teste). A regra de dependência é
estritamente **para dentro**: nada na Domain depende de tecnologia.

```
┌─────────────────────────────────────────────────────────────┐
│                       FinanceControl.Api                    │  ← Minimal API + Controllers + Middlewares
│  (BaseApiController, Swagger, ExceptionHandling, CORS,      │
│   CorrelationId, Serilog request logging, HealthChecks)     │
└─────────────────────────┬───────────────────────────────────┘
                          │ depende
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                 FinanceControl.CrossCutting                 │  ← Composição (DI)
│       AddApplicationDependencies / AddInfrastructureDeps    │
└────────────┬───────────────────────────────┬────────────────┘
             │                               │
             ▼                               ▼
┌────────────────────────┐      ┌────────────────────────────┐
│ FinanceControl.        │      │ FinanceControl.            │
│   Application          │      │   Infrastructure           │
│ (CQRS, MediatR, AM,    │      │ (HttpClients: Binance,     │
│  FluentValidation,     │      │  OpenFinance, News;         │
│  Interfaces, DTOs)     │      │  Auth, Caching, Storage)   │
└────────────┬───────────┘      └─────────────┬──────────────┘
             │                                │
             └──────────────┬─────────────────┘
                            ▼
                ┌────────────────────────┐
                │  FinanceControl.Data   │  ← EF Core (DbContext, configs, migrations, repos)
                └────────────┬───────────┘
                             ▼
                ┌────────────────────────┐
                │  FinanceControl.       │  ← Entidades, VOs, Enums, Eventos, Exceptions
                │     Domain             │     (sem dependências externas além da BCL)
                └────────────────────────┘
```

### Regras de dependência

| De                 | Pode referenciar                                 |
|--------------------|--------------------------------------------------|
| **Api**            | Application, CrossCutting                        |
| **CrossCutting**   | Application, Data, Infrastructure                |
| **Application**    | Domain                                           |
| **Infrastructure** | Application, Domain                              |
| **Data**           | Application, Domain                              |
| **Domain**         | (nada além da BCL)                               |

## 2. CQRS com MediatR

Cada feature em `Application/Features/<Feature>` é organizada por
**Commands** (mutações) e **Queries** (leituras):

```
Features/Categories/
├── Commands/
│   ├── CreateCategory/  → CreateCategoryCommand + Handler + Validator
│   ├── UpdateCategory/
│   └── DeleteCategory/
├── Queries/
│   ├── GetCategoryById/
│   └── GetAllCategories/
├── Validators/  (compartilhados, se necessário)
├── Mappings/    (CategoryProfile : AutoMapper.Profile)
└── Dtos/        (CategoryDto, CreateCategoryDto, ...)
```

Pipeline behaviors registrados (na ordem): `ValidationBehavior` →
`LoggingBehavior` → `UnitOfWorkBehavior`.

## 3. Validação - FluentValidation

Todo `Command` e `Query` tem um `AbstractValidator<T>` no mesmo namespace
da feature. O `ValidationBehavior` executa todos os validators do request
e levanta `ValidationException` (capturada pelo middleware global e
convertida em `400 Bad Request` no envelope `ApiResponse<T>`).

## 4. Mapper - AutoMapper

Os `Profile` ficam **na camada Application** (em `Features/<X>/Mappings/`).
Registro automático via `services.AddAutoMapper(applicationAssembly)`.

## 5. Logging - Serilog fortemente tipado

Configurado em `Program.cs` via `ReadFrom.Configuration` (lê `Serilog:*` do
`appsettings.json`). Sinks: Console + File rolling diário.
Todos os logs incluem `CorrelationId` (injetado pelo `CorrelationIdMiddleware`),
`Application`, `MachineName` e `ThreadId`. Para logs estruturados nos
handlers, usar `ILogger<T>` com message templates (`{@Variable}`).

## 6. Middlewares (na API, pasta `Middlewares/`)

| Ordem | Middleware                       | Responsabilidade                        |
|-------|----------------------------------|------------------------------------------|
| 1     | `UseSerilogRequestLogging`       | Logging estruturado de cada request      |
| 2     | `ExceptionHandlingMiddleware`    | Mapeia exceções → `ApiResponse<T>` 4xx/5xx |
| 3     | `CorrelationIdMiddleware`        | `X-Correlation-Id` por request           |
| 4     | `UseHttpsRedirection`            | Força HTTPS                              |
| 5     | `UseCors("FinanceControlPolicy")`| CORS para o frontend (localhost:3000)    |

## 7. BaseControllerApi

Todos os controllers herdam de `BaseApiController`, que:

- Define `[ApiController]` + `[Route("api/v1/[controller]")]`
- Expõe `Mediator` (lazy)
- Padroniza responses: `OkResponse`, `CreatedResponse`, `NoContentResponse`,
  `BadRequestResponse`, `NotFoundResponse` - todos retornando `ApiResponse<T>`

## 8. Persistência

`FinanceControl.Data` contém o `FinanceControlDbContext` + `IEntityTypeConfiguration<T>`
por entidade, migrations EF Core e repositórios. O contexto é exposto à camada
Application apenas via `IApplicationDbContext` (Dependency Inversion).

Provider padrão: **SQL Server LocalDB** em dev; **SQLite** disponível como
provider alternativo (chaveado por `Database:Provider` no `appsettings.json`).

## 9. Frontend - Color Admin

O frontend usa o template **Color Admin** do SeanThemes
(https://seantheme.com/color-admin/) em sua variante React.
A estrutura visual (sidebar fixa à esquerda, header escuro, dashboards com
widgets e ApexCharts) deve ser seguida **à risca**. Detalhes no Sprint 9.

## 10. Integrações externas

| Integração      | Lib / Endpoint                                  | Sprint |
|-----------------|-------------------------------------------------|--------|
| Binance         | `Binance.Net` (NuGet) - chaves API por usuário  | 6      |
| Cotações cripto | CoinGecko REST (fallback)                       | 6      |
| Open Finance BR | Mock + sandbox dos bancos parceiros             | 7      |
| Notícias        | RSS feeds (CoinDesk, CoinTelegraph, InfoMoney)  | 8      |
