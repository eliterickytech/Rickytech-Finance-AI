# Sprint 12 - Tasks

> Estimativa: **44 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S12-01 - Cobertura de testes (8 pts)
- [ ] Coletar cobertura com `coverlet` _(coleta `XPlat Code Coverage` no CI, mas...)_
- [ ] Subir testes faltantes para alcançar 70% — **só 4 testes de domínio**

## US-S12-02 - Testes E2E (Playwright) (8 pts)
- [ ] Setup Playwright + fixtures — **não configurado**
- [ ] Cenários: login, CRUD Categoria, CRUD Receita, dashboard

## US-S12-03 - Load test (k6) (3 pts)
- [ ] Script `k6/scenario-cruds.js` — **não existe**
- [ ] Validar p95 < 500ms

## US-S12-04 - Autenticação JWT (5 pts)
- [x] Login endpoint _(falta endpoint de **refresh** — só há campo `RefreshToken` no DTO)_
- [x] Roles Admin/User no token (`ClaimTypes.Role` em `JwtTokenService`)
- [ ] `[Authorize]` nos endpoints — **não aplicado nos controllers**

## US-S12-05 - Segurança (3 pts)
- [x] Rate limit (FixedWindow) — limiters `global` e `auth` em `ApiDependencyInjection`
- [ ] HSTS + CSP + headers de segurança — **não encontrados**

## US-S12-06 - Observabilidade (5 pts)
- [ ] OpenTelemetry traces — **não configurado**
- [x] Serilog + Seq — serviço `seq` no `docker-compose` (sink Serilog a confirmar)

## US-S12-07 - Dockerização (5 pts)
- [x] Dockerfile API multi-stage
- [x] Dockerfile Frontend (nginx)
- [x] docker-compose com SQL Server + Seq

## US-S12-08 - CI/CD completo (4 pts)
- [ ] Pipeline build → test → publish → push image → deploy — **`ci.yml` faz só build + test**
- [ ] Environments: dev / staging / prod

## US-S12-09 - Runbook + Release (3 pts)
- [x] `docs/runbook.md` (start/stop, troubleshooting, migrations)
- [ ] CHANGELOG.md + tag `v1.0.0` — _`CHANGELOG.md` existe, mas **tag `v1.0.0` não foi criada**_
