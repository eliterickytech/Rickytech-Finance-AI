# Sprint 1 - Domain & CRUD de Categorias

## Objetivo
Estabelecer o coração do Domain (entidades base, value objects, enums)
e entregar o **primeiro CRUD vertical** ponta-a-ponta: Categorias.

## Escopo IN
- Configurar `FinanceControlDbContext` em `FinanceControl.Data`.
- Primeira migration EF Core (`20260601_0000_Sprint01_InitialSchema`).
- Implementar `IApplicationDbContext` em Data; registrar provider via `Database:Provider`.
- Criar entidade **Category** (Domain) + ValueObjects (`HexColor`, `IconName`).
- Enum `CategoryType { Income, Expense, Both }`.
- CRUD completo via MediatR:
  - `CreateCategoryCommand`, `UpdateCategoryCommand`, `DeleteCategoryCommand`
  - `GetCategoryByIdQuery`, `GetAllCategoriesQuery` (com filtro por tipo)
- Validators FluentValidation para cada command.
- AutoMapper profile `CategoryProfile`.
- `CategoriasController : BaseApiController` com 5 endpoints.
- Seed de categorias padrão (Alimentação, Moradia, Transporte, Salário, Investimentos, ...).
- Testes unitários (Domain + Application) e integração (CRUD).

## Escopo OUT
- Categoria hierárquica (subcategoria) — fica para um spike futuro.
- Frontend (Sprint 10).

## Decisões
- IDs são `Guid` gerados na Domain (não no banco).
- Soft delete: `Category` herda de `BaseEntity` que já tem `IsDeleted`.
- Filtro automático no DbContext para `IsDeleted = false` (HasQueryFilter).

## Critério de pronto (DoD)
- `POST /api/v1/categorias` cria categoria e responde `201` com envelope.
- `GET /api/v1/categorias?tipo=Despesa` retorna lista filtrada.
- `PUT /api/v1/categorias/{id}` e `DELETE /api/v1/categorias/{id}` funcionam.
- Migration aplicada em LocalDB e SQLite.
- Cobertura de testes >= 80% nos handlers.
