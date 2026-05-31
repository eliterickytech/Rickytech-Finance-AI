# Sprint 12 - Tasks

> Estimativa: **44 pts**

## US-S12-01 - Cobertura de testes (8 pts)
- [ ] Coletar cobertura com `coverlet`
- [ ] Subir testes faltantes para alcançar 70%

## US-S12-02 - Testes E2E (Playwright) (8 pts)
- [ ] Setup Playwright + fixtures
- [ ] Cenários: login, CRUD Categoria, CRUD Receita, dashboard

## US-S12-03 - Load test (k6) (3 pts)
- [ ] Script `k6/scenario-cruds.js`
- [ ] Validar p95 < 500ms

## US-S12-04 - Autenticação JWT (5 pts)
- [ ] Login endpoint + refresh
- [ ] Roles Admin/User
- [ ] `[Authorize]` nos endpoints

## US-S12-05 - Segurança (3 pts)
- [ ] Rate limit (FixedWindow)
- [ ] HSTS + CSP + headers de segurança

## US-S12-06 - Observabilidade (5 pts)
- [ ] OpenTelemetry traces
- [ ] Serilog para Seq (dev) / App Insights (prod)

## US-S12-07 - Dockerização (5 pts)
- [ ] Dockerfile API multi-stage
- [ ] Dockerfile Frontend (nginx)
- [ ] docker-compose com SQL Server + Seq

## US-S12-08 - CI/CD completo (4 pts)
- [ ] Pipeline build → test → publish → push image → deploy
- [ ] Environments: dev / staging / prod

## US-S12-09 - Runbook + Release (3 pts)
- [ ] `docs/runbook.md` (start/stop, troubleshooting, migrations)
- [ ] CHANGELOG.md + tag `v1.0.0`
