# Sprint 0 - Progress

> Atualize esta página **diariamente** (data → o que andou → o que travou).
>
> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-25 (Sprint start)
- Estrutura inicial de pastas criada via cowork.
- `FinanceControl.sln`, `Directory.Build.props` e os 6 `.csproj` placeholders criados.
- `Program.cs` esqueleto com Serilog, DI e middleware base.
- `BaseApiController`, `ApiResponse<T>`, middlewares de Exception e CorrelationId escritos.
- ValidationBehavior do MediatR criado.

### 2026-05-31 (Auditoria de código)
- Revisão completa do código-fonte para sincronizar a documentação com a realidade.
- Sprint 0 concluída: solution, DI, middlewares, Serilog, Swagger/CORS/Health e pipeline behaviors presentes.
- CI/CD (`.github/workflows/ci.yml`) já existe → US-S0-10 marcada como implementada.

## Status das US

| US                                  | Status          |
|-------------------------------------|-----------------|
| US-S0-01 Solution e projetos        | ✅ Implementado |
| US-S0-02 Pacotes NuGet base         | ✅ Implementado |
| US-S0-03 BaseApiController          | ✅ Implementado |
| US-S0-04 Middlewares                | ✅ Implementado |
| US-S0-05 Serilog                    | ✅ Implementado |
| US-S0-06 CrossCutting DI            | ✅ Implementado |
| US-S0-07 Pipeline behaviors         | ✅ Implementado |
| US-S0-08 Swagger + CORS + Health    | ✅ Implementado |
| US-S0-09 Interfaces Application     | ✅ Implementado |
| US-S0-10 CI/CD                      | ✅ Implementado |
| US-S0-11 Doc SDD                    | ✅ Implementado |

## Bloqueios
_(nenhum)_

## Demo prevista
- Subir API em `https://localhost:5001`
- Hit em `/health`
- Hit em `/swagger`
- Mostrar log estruturado no console com `CorrelationId`
