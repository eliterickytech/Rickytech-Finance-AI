# Sprint 0 - Progress

> Atualize esta página **diariamente** (data → o que andou → o que travou).

## Diário

### 2026-05-25 (Sprint start)
- Estrutura inicial de pastas criada via cowork.
- `FinanceControl.sln`, `Directory.Build.props` e os 6 `.csproj` placeholders criados.
- `Program.cs` esqueleto com Serilog, DI e middleware base.
- `BaseApiController`, `ApiResponse<T>`, middlewares de Exception e CorrelationId escritos.
- ValidationBehavior do MediatR criado.

### (próximo dia)
- ...

## Status das US

| US                                  | Status      |
|-------------------------------------|-------------|
| US-S0-01 Solution e projetos        | ⏳ em curso |
| US-S0-02 Pacotes NuGet base         | ⏳ em curso |
| US-S0-03 BaseApiController          | ✅ inicial  |
| US-S0-04 Middlewares                | ✅ inicial  |
| US-S0-05 Serilog                    | ⏳ em curso |
| US-S0-06 CrossCutting DI            | ✅ inicial  |
| US-S0-07 Pipeline behaviors         | ✅ inicial  |
| US-S0-08 Swagger + CORS + Health    | ⏳ em curso |
| US-S0-09 Interfaces Application     | ✅ inicial  |
| US-S0-10 CI/CD                      | ⏸ pendente |
| US-S0-11 Doc SDD                    | ⏳ em curso |

## Bloqueios
_(nenhum até o momento)_

## Demo prevista
- Subir API em `https://localhost:5001`
- Hit em `/health`
- Hit em `/swagger`
- Mostrar log estruturado no console com `CorrelationId`
