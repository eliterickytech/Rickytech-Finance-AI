# Sprint 0 - Setup & Arquitetura Base

## Objetivo
Preparar o repositório, a solução .NET 10 e a estrutura de Clean Architecture
de modo que o Sprint 1 já possa começar a entregar features sem retrabalho
estrutural.

## Escopo IN
- Criar `FinanceControl.sln` com os 6 projetos de produção + 3 de testes.
- Configurar `Directory.Build.props` (net10.0, nullable, warnings-as-errors).
- Adicionar pacotes NuGet base: MediatR, AutoMapper, FluentValidation, EF Core 10, Serilog.
- Configurar `Program.cs` em Minimal API com pipeline mínimo (Serilog, Swagger, CORS, HealthChecks).
- Implementar `BaseApiController`, `ApiResponse<T>`, `ExceptionHandlingMiddleware`, `CorrelationIdMiddleware`.
- Criar `IApplicationDbContext`, `ICurrentUserService`, `ValidationBehavior<,>`.
- Configurar Serilog fortemente tipado (lendo de `appsettings.json`).
- Configurar dois projetos de DI em CrossCutting: `AddApplicationDependencies` e `AddInfrastructureDependencies`.
- Configurar CI básico (build + test) no GitHub Actions / Azure DevOps.

## Escopo OUT (vai para sprints futuros)
- Nenhum CRUD de entidade real.
- Frontend (começa no Sprint 9).
- Integrações externas.

## Decisões
- **Provider de banco**: SQL Server LocalDB em dev; SQLite como alternativa.
  Definido via `Database:Provider` no `appsettings.json`.
- **Versionamento de API**: prefixo `/api/v1` desde o início.
- **JSON**: usar `System.Text.Json` com `camelCase`.
- **Cultura**: API trabalha internamente em `pt-BR` e UTC para datas.

## Riscos
| Risco                                            | Mitigação                                |
|--------------------------------------------------|-------------------------------------------|
| .NET 10 SDK não disponível na CI                 | Pin de versão via `global.json`           |
| Binance.Net não compatível com .NET 10           | Validar versão mínima no Sprint 6         |

## Critério de pronto (DoD)
- `dotnet build` verde para a solução inteira.
- Swagger acessível em `https://localhost:5001/swagger`.
- Health check OK em `/health`.
- Log no Console mostra entrada/saída de cada request com `CorrelationId`.
- README e ARCHITECTURE renderizando corretamente.
