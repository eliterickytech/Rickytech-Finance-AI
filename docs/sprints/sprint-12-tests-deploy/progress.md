# Sprint 12 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- Autenticação JWT: `IJwtTokenService`/`JwtTokenService`, `LoginCommand`, `AuthController` (endpoint `login`). ❌ Falta endpoint de **registro** no backend (a tela `Auth/Register` existe no frontend, mas não há `RegisterCommand`).
- Dockerização: `backend/Dockerfile` + `docker-compose.yml` presentes.
- CI: `.github/workflows/ci.yml` presente.
- 🟡 Testes: apenas 4 testes de domínio (`BankTests`, `CategoryTests`, `InvestmentTests`, `RecurrenceProjectorTests`). Sem testes de Application/integração de API; sem E2E/Playwright; sem load test.
- ❌ Observabilidade (Seq), runbook e release v1.0.0 pendentes.

## Status das US

| US                                | Status          |
|-----------------------------------|-----------------|
| US-S12-01 Cobertura de testes     | 🟡 Parcial (só domínio) |
| US-S12-02 Testes E2E              | ❌ Pendente |
| US-S12-03 Load test               | ❌ Pendente |
| US-S12-04 Autenticação JWT        | 🟡 Parcial (login + roles ok; faltam refresh, `[Authorize]` e register) |
| US-S12-05 Segurança               | 🟡 Parcial (rate limit FixedWindow + criptografia de chaves; faltam HSTS/CSP) |
| US-S12-06 Observabilidade         | 🟡 Parcial (Serilog ok; falta Seq/dashboards) |
| US-S12-07 Dockerização            | ✅ Implementado |
| US-S12-08 CI/CD completo          | 🟡 Parcial (workflow existe; validar pipeline) |
| US-S12-09 Runbook + Release       | ❌ Pendente |

## Bloqueios
- **Migration do EF Core ausente** impede `docker compose up` funcional com banco (ver Sprint 1).

## Demo prevista (Release v1.0.0)
- Subir ambiente completo via `docker compose up`
- Rodar suite Playwright até o fim
- Mostrar dashboards do Seq com requests com CorrelationId
- Tag v1.0.0 publicada
