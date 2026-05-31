# Sprint 0 - Spec técnica

## 1. Solution layout

```
backend/
├── FinanceControl.sln
├── Directory.Build.props
└── src/
    ├── FinanceControl.Api/             → Microsoft.NET.Sdk.Web (Minimal API)
    ├── FinanceControl.Application/     → Microsoft.NET.Sdk
    ├── FinanceControl.Domain/          → Microsoft.NET.Sdk
    ├── FinanceControl.CrossCutting/    → Microsoft.NET.Sdk
    ├── FinanceControl.Data/            → Microsoft.NET.Sdk
    └── FinanceControl.Infrastructure/  → Microsoft.NET.Sdk
```

## 2. Dependências entre projetos

```
Api  ──▶ Application + CrossCutting
CrossCutting ──▶ Application + Data + Infrastructure
Application ──▶ Domain
Data ──▶ Domain + Application
Infrastructure ──▶ Domain + Application
```

## 3. Pacotes NuGet por projeto

### FinanceControl.Api
- `Microsoft.AspNetCore.OpenApi` 10.0.0
- `Swashbuckle.AspNetCore` 6.7.3
- `FluentValidation.AspNetCore` 11.3.0
- `Serilog.AspNetCore` 8.0.3
- `AspNetCore.HealthChecks.UI.Client` 8.0.1

### FinanceControl.Application
- `MediatR` 12.4.1
- `AutoMapper` 13.0.1 + `AutoMapper.Extensions.Microsoft.DependencyInjection` 12.0.1
- `FluentValidation` 11.10.0

### FinanceControl.Data
- `Microsoft.EntityFrameworkCore` 10.0.0 (+ `Relational`, `SqlServer`, `Sqlite`, `Design`)

### FinanceControl.Infrastructure
- `Microsoft.Extensions.Http` 10.0.0
- `Polly` 8.4.2 + `Microsoft.Extensions.Http.Resilience` 9.0.0
- `Binance.Net` 10.18.0
- `CoinGecko.Api` 1.3.0

### FinanceControl.CrossCutting
- `Microsoft.Extensions.DependencyInjection` 10.0.0
- `Serilog` 4.1.0
- `Serilog.Extensions.Hosting` 8.0.0

## 4. Configuração do Serilog (`appsettings.json`)

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": { "Microsoft": "Warning" }
  },
  "WriteTo": [
    { "Name": "Console" },
    { "Name": "File", "Args": { "path": "logs/finance-control-.log", "rollingInterval": "Day", "retainedFileCountLimit": 14 } }
  ],
  "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
}
```

## 5. Pipeline final do Program.cs (resumido)

```
WebApplication.CreateBuilder
  └─ Host.UseSerilog
  └─ services.AddApplicationDependencies
  └─ services.AddInfrastructureDependencies
  └─ services.AddApiDependencies

app
  ├─ UseSerilogRequestLogging
  ├─ UseMiddleware<ExceptionHandlingMiddleware>
  ├─ UseMiddleware<CorrelationIdMiddleware>
  ├─ UseSwagger / UseSwaggerUI (dev)
  ├─ UseHttpsRedirection
  ├─ UseCors("FinanceControlPolicy")
  ├─ MapControllers
  └─ MapHealthChecks("/health")
```

## 6. Contratos de teste

- `FinanceControl.UnitTests` referencia Application + Domain.
- `FinanceControl.IntegrationTests` referencia Data + Application + Infrastructure (usa SQLite in-memory).
- `FinanceControl.ApiTests` usa `WebApplicationFactory<Program>` (Program declarado como `public partial class`).
