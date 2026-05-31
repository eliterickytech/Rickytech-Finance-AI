# Sprint 12 - Testes, QA, Hardening e Deploy

## Objetivo
Consolidar a base de testes, fechar débito técnico, fortalecer
segurança e operacional, e publicar o Release 1.0.

## Escopo IN
- Atingir cobertura **≥ 70%** em Application e Domain.
- Testes E2E com **Playwright** (login → CRUDs → integração mockada).
- **Load test** com k6 (cenário: 50 RPS por 5 min em CRUDs).
- Segurança:
  - Autenticação JWT (com refresh token).
  - Roles: Admin, User.
  - Helmet + rate limit + content security policy.
- Observabilidade:
  - Serilog → Seq (dev) ou Application Insights (prod).
  - OpenTelemetry traces.
- Migrations production-ready (script idempotente).
- CI/CD com pipeline completo (build → test → publish → deploy).
- Docker:
  - `Dockerfile` para API
  - `Dockerfile` para Frontend (nginx)
  - `docker-compose.yml` (API + SQL Server + Seq)
- Documentação operacional (`docs/runbook.md`).
- Release tag **v1.0.0**.

## Critério de pronto (DoD)
- `dotnet test` verde com cobertura >= 70%.
- Playwright suite verde no CI.
- Dockerfile builda e roda local.
- App deployado em ambiente de staging.
- Smoke test (`/health`, listar categorias, sync mock) verde no staging.
