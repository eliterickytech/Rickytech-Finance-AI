# Finance Control

Aplicativo de **controle financeiro pessoal e cripto** com:

- CRUDs de Receitas, Despesas, Categorias, Bancos e Investimentos
- Projeções de lucros e fluxo de caixa futuro
- Integrações com **Binance** (BTC, ETH, ADA, SOL e demais ativos), bancos via **Open Finance Brasil** e contas correntes
- Painel de **notícias** do mercado financeiro tradicional e do mercado cripto
- Banco de dados local em SQL (SQL Server LocalDB ou SQLite)

## Stack

| Camada      | Tecnologia                                                                |
|-------------|---------------------------------------------------------------------------|
| Backend     | **C# .NET 10**, Minimal API, CQRS (MediatR), Clean Architecture           |
| Validação   | **FluentValidation** (pipeline behavior do MediatR)                       |
| Mapper      | **AutoMapper** (perfis na camada Application)                             |
| Logging     | **Serilog** fortemente tipado (sinks Console + File rolling)              |
| ORM         | **Entity Framework Core 10** (SQL Server / SQLite)                        |
| Frontend    | **React 18 + TypeScript + Vite**, layout **Color Admin** (SeanThemes)     |
| Testes      | xUnit, FluentAssertions, NSubstitute, Testcontainers                      |

## Estrutura do repositório

```
Finance Control/
├── backend/                              # Solução .NET 10
│   ├── FinanceControl.sln
│   ├── Directory.Build.props             # Configs globais (net10.0, nullable, warnings as errors)
│   ├── src/
│   │   ├── FinanceControl.Api/           # Minimal API + Controllers + Middlewares
│   │   ├── FinanceControl.Application/   # CQRS (commands/queries), validators, mappings, interfaces
│   │   ├── FinanceControl.Domain/        # Entidades, value objects, enums, eventos, exceptions
│   │   ├── FinanceControl.CrossCutting/  # DI (Application + Infrastructure), Serilog, configs
│   │   ├── FinanceControl.Data/          # EF Core: DbContext, configurations, migrations, repos
│   │   └── FinanceControl.Infrastructure/# Integrações externas (Binance, OpenFinance, News)
│   └── tests/
│       ├── FinanceControl.UnitTests/
│       ├── FinanceControl.IntegrationTests/
│       └── FinanceControl.ApiTests/
│
├── frontend/                             # React 18 + TypeScript + Vite (Color Admin)
│   └── src/
│       ├── components/{layout,common,widgets,charts}
│       ├── pages/{Dashboard,Categories,Banks,Incomes,Expenses,Investments,...}
│       ├── routes/  services/api/  hooks/  store/  theme/color-admin/
│       └── assets/{css,scss,images,fonts}
│
└── docs/                                 # SDD - Software Design Documentation
    ├── ARCHITECTURE.md
    ├── ROADMAP.md
    ├── SPEC.md
    ├── architecture/
    ├── api-contracts/
    ├── diagrams/
    └── sprints/                          # plan.md + tasks.md + progress.md + spec.md por sprint
        ├── sprint-00-setup/
        ├── sprint-01-categories/
        ├── sprint-02-banks/
        ├── sprint-03-incomes-expenses/
        ├── sprint-04-investments/
        ├── sprint-05-projections/
        ├── sprint-06-binance/
        ├── sprint-07-openfinance/
        ├── sprint-08-news/
        ├── sprint-09-frontend-setup/
        ├── sprint-10-frontend-cruds/
        ├── sprint-11-frontend-integrations/
        └── sprint-12-tests-deploy/
```

## Documentação

| Documento | O que contém |
|-----------|--------------|
| [`docs/SPEC.md`](docs/SPEC.md) | Especificação técnica global — requisitos funcionais e não-funcionais |
| [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md) | Diagramas de camadas, fluxo de uma request, regras de Clean Architecture |
| [`docs/ROADMAP.md`](docs/ROADMAP.md) | Roadmap completo dividido em 13 sprints |
| `docs/sprints/sprint-XX/plan.md`     | Objetivos, escopo e decisões de design daquele sprint |
| `docs/sprints/sprint-XX/tasks.md`    | Quebra de tarefas (US, AC, estimativa) |
| `docs/sprints/sprint-XX/progress.md` | Diário do sprint, bloqueios, demos |
| `docs/sprints/sprint-XX/spec.md`     | Especificação técnica detalhada do escopo do sprint |

## Como rodar (após Sprint 0 + Sprint 1)

```bash
# Backend
cd backend
dotnet restore
dotnet ef database update -p src/FinanceControl.Data -s src/FinanceControl.Api
dotnet run --project src/FinanceControl.Api

# Frontend
cd ../frontend
npm install
npm run dev
```

## Autor

Ricardo Perdigão / Rickytech.
