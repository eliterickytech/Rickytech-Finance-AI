# Sprint 12 - Spec técnica (Testes, Hardening, Deploy)

## Cobertura mínima

| Camada       | Target |
|--------------|--------|
| Domain       | 90%    |
| Application  | 80%    |
| Data         | 60%    |
| Infrastructure | 50%  |
| Api          | 70%    |

## Stack de testes

- **Unit**: xUnit + FluentAssertions + NSubstitute
- **Integration**: Testcontainers (SQL Server) + WireMock.Net (Binance/OFI/RSS)
- **API**: WebApplicationFactory<Program>
- **E2E**: Playwright (TS)
- **Load**: k6

## Autenticação JWT

```
POST /api/v1/auth/login
  body: { "email": "...", "password": "..." }
  → 200 { "accessToken": "...", "refreshToken": "...", "expiresIn": 3600 }

POST /api/v1/auth/refresh
  body: { "refreshToken": "..." }
  → 200 { ... }
```

Header obrigatório: `Authorization: Bearer <accessToken>` em todos os endpoints
exceto `/health`, `/swagger`, `/auth/*`.

## Rate limit

- FixedWindow: 100 req/min por IP + por usuário autenticado.
- Endpoints sensíveis (`/auth/login`): 10 req/min.

## Docker

```dockerfile
# backend/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish src/FinanceControl.Api/FinanceControl.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinanceControl.Api.dll"]
```

```yaml
# docker-compose.yml
services:
  api:
    build: ./backend
    ports: ["8080:8080"]
    environment:
      ConnectionStrings__FinanceControlDb: "Server=sql;..."
    depends_on: [sql, seq]
  frontend:
    build: ./frontend
    ports: ["3000:80"]
  sql:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: "Y"
      MSSQL_SA_PASSWORD: "Finance@2026"
    ports: ["1433:1433"]
  seq:
    image: datalust/seq:latest
    environment:
      ACCEPT_EULA: "Y"
    ports: ["5341:80"]
```

## OpenTelemetry

- Instrumentação: AspNetCore + HttpClient + EF Core
- Exporter: OTLP → Jaeger (dev) / Application Insights (prod)
- Spans nomeados conforme `MediatR.<Command>`

## Runbook (highlights)

- Subir ambiente: `docker compose up -d`
- Aplicar migrations: `dotnet ef database update`
- Logs: `docker logs <container>` ou Seq em `http://localhost:5341`
- Rollback de migration: `dotnet ef database update <PreviousMigrationName>`
