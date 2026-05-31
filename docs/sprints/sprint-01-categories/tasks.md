# Sprint 1 - Tasks

> Estimativa total: **31 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S1-01 - DbContext + provider (5 pts)
- [x] Criar `FinanceControlDbContext : DbContext, IApplicationDbContext`
- [x] Registrar em `AddInfrastructureDependencies` chaveando por `Database:Provider`
- [x] `AuditInterceptor` para preencher `CreatedAt` / `UpdatedAt`
- [x] HasQueryFilter global para `IsDeleted = false`

**AC:** App sobe e cria base vazia em LocalDB e em SQLite.

## US-S1-02 - Entidade Category (3 pts)
- [x] `Category : BaseEntity` com Name, Type, Color, Icon, ParentCategoryId, Active
- [x] ValueObject `HexColor`
- [x] Enum `CategoryType { Income = 1, Expense = 2, Both = 3 }`
- [x] `CategoryConfiguration : IEntityTypeConfiguration<Category>`

**AC:** Snapshot da migration mostra tabela `Categories` com colunas corretas.

## US-S1-03 - Migration inicial (3 pts)
- [ ] `dotnet ef migrations add Sprint01_InitialSchema -p Data -s Api` — **NÃO gerada**
- [ ] Aplicar em LocalDB
- [ ] Documentar no `progress.md`

## US-S1-04 - Commands CRUD (8 pts)
- [x] `CreateCategoryCommand` + Handler + Validator + Dto
- [x] `UpdateCategoryCommand` + Handler + Validator + Dto
- [x] `DeleteCategoryCommand` + Handler (soft delete)
- [x] AutoMapper `CategoryProfile` (Entity ↔ Dto)

## US-S1-05 - Queries (3 pts)
- [x] `GetCategoryByIdQuery` + Handler (404 via `NotFoundException`)
- [x] `GetAllCategoriesQuery` com filtros (Type, Active, search) e paginação

## US-S1-06 - Controller (3 pts)
- [x] `CategoriasController : BaseApiController` com 5 endpoints REST
- [x] Atributos `[ProducesResponseType]` por status

## US-S1-07 - Seed (2 pts)
- [x] Seed de categorias default em pt-BR (despesas comuns + receitas comuns)

## US-S1-08 - Testes (4 pts)
- [ ] Unit tests dos validators e handlers (xUnit + FluentAssertions + NSubstitute) — **só existe teste de entidade `CategoryTests`**
- [ ] Integration test do CRUD usando SQLite in-memory
- [ ] API test do happy-path (`WebApplicationFactory`)
