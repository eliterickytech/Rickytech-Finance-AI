# Sprint 1 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- CRUD de Categorias completo: entidade `Category`, `CategoryType`, value object `HexColor`, CQRS (Create/Update/Delete + GetAll/GetById), `CategoriasController`, validators e seed (`DatabaseSeeder`).
- Testes de domínio presentes (`CategoryTests`).
- ⚠️ **Migration inicial NÃO foi gerada** — pasta `Migrations/` só tem `.gitkeep`. Bloqueia subir o banco.

## Status das US

| US                                   | Status          |
|--------------------------------------|-----------------|
| US-S1-01 DbContext + provider        | ✅ Implementado |
| US-S1-02 Entidade Category           | ✅ Implementado |
| US-S1-03 Migration inicial           | ❌ Pendente (não gerada) |
| US-S1-04 Commands CRUD               | ✅ Implementado |
| US-S1-05 Queries                     | ✅ Implementado |
| US-S1-06 Controller                  | ✅ Implementado |
| US-S1-07 Seed                        | ✅ Implementado |
| US-S1-08 Testes                      | 🟡 Parcial (só `CategoryTests` de domínio; faltam handler/integração/API) |

## Bloqueios
- **Migration do EF Core ausente** — necessário rodar `dotnet ef migrations add InitialCreate` antes da demo.

## Demo prevista
- Swagger: criar / listar / atualizar / deletar categorias
- Mostrar tabela `Categories` no LocalDB com o seed aplicado
