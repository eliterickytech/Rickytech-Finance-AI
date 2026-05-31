# Sprint 1 - Tasks

> Estimativa total: **31 pts**

## US-S1-01 - DbContext + provider (5 pts)
- [ ] Criar `FinanceControlDbContext : DbContext, IApplicationDbContext`
- [ ] Registrar em `AddInfrastructureDependencies` chaveando por `Database:Provider`
- [ ] `AuditInterceptor` para preencher `CreatedAt` / `UpdatedAt`
- [ ] HasQueryFilter global para `IsDeleted = false`

**AC:** App sobe e cria base vazia em LocalDB e em SQLite.

## US-S1-02 - Entidade Category (3 pts)
- [ ] `Category : BaseEntity` com Name, Type, Color, Icon, ParentCategoryId, Active
- [ ] ValueObject `HexColor`
- [ ] Enum `CategoryType { Income = 1, Expense = 2, Both = 3 }`
- [ ] `CategoryConfiguration : IEntityTypeConfiguration<Category>`

**AC:** Snapshot da migration mostra tabela `Categories` com colunas corretas.

## US-S1-03 - Migration inicial (3 pts)
- [ ] `dotnet ef migrations add Sprint01_InitialSchema -p Data -s Api`
- [ ] Aplicar em LocalDB
- [ ] Documentar no `progress.md`

## US-S1-04 - Commands CRUD (8 pts)
- [ ] `CreateCategoryCommand` + Handler + Validator + Dto
- [ ] `UpdateCategoryCommand` + Handler + Validator + Dto
- [ ] `DeleteCategoryCommand` + Handler (soft delete)
- [ ] AutoMapper `CategoryProfile` (Entity ↔ Dto)

## US-S1-05 - Queries (3 pts)
- [ ] `GetCategoryByIdQuery` + Handler (404 via `NotFoundException`)
- [ ] `GetAllCategoriesQuery` com filtros (Type, Active, search) e paginação

## US-S1-06 - Controller (3 pts)
- [ ] `CategoriasController : BaseApiController` com 5 endpoints REST
- [ ] Atributos `[ProducesResponseType]` por status

## US-S1-07 - Seed (2 pts)
- [ ] Seed de categorias default em pt-BR (despesas comuns + receitas comuns)

## US-S1-08 - Testes (4 pts)
- [ ] Unit tests dos validators e handlers (xUnit + FluentAssertions + NSubstitute)
- [ ] Integration test do CRUD usando SQLite in-memory
- [ ] API test do happy-path (`WebApplicationFactory`)
