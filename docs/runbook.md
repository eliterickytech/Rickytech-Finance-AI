# Runbook Operacional - Finance Control

## Subir ambiente local com Docker

```bash
docker compose up -d
docker compose logs -f api
```

Serviços expostos:
- API:       http://localhost:5001/swagger
- Frontend:  http://localhost:3000
- SQL Server: localhost:1433 (sa / Finance@2026)
- Seq logs:  http://localhost:5341

## Subir backend localmente (sem Docker)

```bash
cd backend
dotnet restore
dotnet ef database update -p src/FinanceControl.Data -s src/FinanceControl.Api
dotnet run --project src/FinanceControl.Api
```

A primeira execução roda migrations + seed (categorias default + bancos default).

## Subir frontend localmente

```bash
cd frontend
npm install
cp .env.example .env
npm run dev
# http://localhost:3000
```

## Migrations

Criar nova migration:
```bash
dotnet ef migrations add NomeDaMigration -p backend/src/FinanceControl.Data -s backend/src/FinanceControl.Api
```

Aplicar:
```bash
dotnet ef database update -p backend/src/FinanceControl.Data -s backend/src/FinanceControl.Api
```

Reverter para uma migration anterior:
```bash
dotnet ef database update PreviousMigrationName -p backend/src/FinanceControl.Data -s backend/src/FinanceControl.Api
```

## Provider alternativo (SQLite)

Em `appsettings.Development.json` defina:
```json
{ "Database": { "Provider": "Sqlite" } }
```

## Login MVP

```
POST /api/v1/auth/login
{ "email": "rickyteck@hotmail.com", "password": "FinanceControl@2026" }
```

Substituir por ASP.NET Core Identity + tabela Users no pós-MVP.

## Integrações

### Binance
1. Gerar par de chaves API (somente leitura) em https://www.binance.com/en/my/settings/api-management
2. POST `/api/v1/integracoes/binance` com `{ "apiKey": "...", "apiSecret": "..." }`
3. POST `/api/v1/integracoes/binance/sync` para importar

### Open Finance Brasil
- Em dev: provider em modo `Mock` retorna transações de exemplo
- Para sandbox real: definir `Integrations:OpenFinance:Mode = Sandbox` + credenciais

## Troubleshooting

| Sintoma                                | Ação                                                              |
|----------------------------------------|--------------------------------------------------------------------|
| `dotnet ef` não encontrado             | `dotnet tool install --global dotnet-ef --version 10.*`           |
| Erro de conexão SQL Server             | Conferir `ConnectionStrings:FinanceControlDb` e firewall          |
| Notícias não aparecem                  | Aguardar `NewsRefreshHostedService` (a cada 15 min); ver logs Seq |
| Cotações cripto não atualizam          | Verificar conectividade com `api.binance.com`                     |

## Observabilidade

- Logs estruturados Serilog → Console + arquivo `logs/finance-control-*.log` + Seq em prod
- Todos os requests carregam header `X-Correlation-Id`
- Cada Command/Query é logado pelo `LoggingBehavior` com elapsed time

## Roadmap de dependências

### AutoMapper → Mapster
A linha 13.x/14.x do AutoMapper ficou com a CVE-2026-32933 (DoS via recursão
sem limite) **sem patch** — a 14.x foi a última versão MIT e o autor moveu
o projeto para licenciamento comercial nas versões 15.x e 16.x, onde o fix
foi aplicado.

**Mitigação atual** (sem custo, já no código):
- `cfg.ForAllMaps((_, e) => e.MaxDepth(64))` em `ApplicationDependencyInjection`
  bloqueia o vetor de stack overflow da CVE.
- `WarningsNotAsErrors=NU1903` no `Directory.Build.props` mantém o aviso
  visível sem bloquear o build.

**Plano definitivo:** migrar para **Mapster** (free, MIT, mantido,
performance comparável ou melhor). A migração é mecânica: trocar
`Profile : Profile { CreateMap<A, B>() }` por `TypeAdapterConfig<A, B>.NewConfig()`
e injetar `IMapper` da `Mapster.DependencyInjection`. Tarefa estimada
em ~1 sprint isolado quando fizer sentido.

### Outras dependências
- `System.Security.Cryptography.Xml`: manter sempre na linha 9.0.x mais
  recente (≥ 9.0.15) ou 10.x (≥ 10.0.6) — vem transitivo via EF Core e
  System.IdentityModel.Tokens.Jwt.
- Auditar todo NuGet com `dotnet list package --vulnerable --include-transitive`
  no início de cada sprint.

## Release

```bash
# Tag de release
git tag -a v1.0.0 -m "Finance Control v1.0.0"
git push origin v1.0.0
```
