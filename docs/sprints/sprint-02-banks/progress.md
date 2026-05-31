# Sprint 2 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- CRUD de Bancos completo: entidade `Bank`, enum `BankAccountType`, value object `Money` (multi-moeda), CQRS (Create/Update/Delete + GetAll/GetById), `BancosController`, validators, `BankConfiguration` e seed.
- Testes de domínio presentes (`BankTests`).
- ⚠️ Migration compartilhada com Sprint 1 ainda não gerada.

## Status das US

| US                          | Status          |
|-----------------------------|-----------------|
| US-S2-01 Entidade Bank      | ✅ Implementado |
| US-S2-02 Migration          | ❌ Pendente (não gerada) |
| US-S2-03 Commands + Queries | ✅ Implementado |
| US-S2-04 Validators         | ✅ Implementado |
| US-S2-05 Controller         | ✅ Implementado |
| US-S2-06 Seed               | ✅ Implementado |
| US-S2-07 Testes             | 🟡 Parcial (só `BankTests` de domínio; faltam integração/API) |

## Bloqueios
- **Migration do EF Core ausente** (ver Sprint 1).

## Demo prevista
- Listar e cadastrar contas bancárias (BRL e cripto) via Swagger
- Mostrar persistência multi-moeda no LocalDB
