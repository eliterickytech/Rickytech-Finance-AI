# Sprint 1 - Spec técnica (Categorias)

## Entidade

```csharp
public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; }
    public HexColor Color { get; private set; } = HexColor.Default;
    public string Icon { get; private set; } = "fa-folder";
    public Guid? ParentCategoryId { get; private set; }
    public Category? Parent { get; private set; }
    public bool Active { get; private set; } = true;

    // factory + métodos de mutação que validam invariantes
    public static Category Create(string name, CategoryType type, HexColor color, string icon) { ... }
    public void Rename(string newName) { ... }
    public void ChangeType(CategoryType newType) { ... }
    public void Disable() { Active = false; MarkAsUpdated(); }
}
```

## DTOs

```csharp
public record CategoryDto(Guid Id, string Name, CategoryType Type, string Color, string Icon, bool Active);
public record CreateCategoryDto(string Name, CategoryType Type, string Color, string Icon);
public record UpdateCategoryDto(Guid Id, string Name, CategoryType Type, string Color, string Icon, bool Active);
```

## Endpoints

| Verbo  | Rota                            | Comando/Query              | Status sucesso |
|--------|---------------------------------|-----------------------------|----------------|
| POST   | `/api/v1/categorias`            | `CreateCategoryCommand`    | 201 Created    |
| GET    | `/api/v1/categorias/{id}`       | `GetCategoryByIdQuery`     | 200 OK         |
| GET    | `/api/v1/categorias`            | `GetAllCategoriesQuery`    | 200 OK         |
| PUT    | `/api/v1/categorias/{id}`       | `UpdateCategoryCommand`    | 200 OK         |
| DELETE | `/api/v1/categorias/{id}`       | `DeleteCategoryCommand`    | 204 No Content |

## Validators (FluentValidation)

```csharp
public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Color).Matches("^#([0-9A-Fa-f]{6})$");
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(50);
    }
}
```

## Seed (categorias default em pt-BR)

- **Receita:** Salário, Freelance, Rendimentos de Investimento, Reembolsos
- **Despesa:** Moradia, Alimentação, Transporte, Saúde, Educação, Lazer, Assinaturas, Impostos
- **Ambos:** Transferência entre contas, Ajustes

## Migration

Nome: `20260601_0000_Sprint01_InitialSchema`
Tabela: `Categories` (Id PK, Name NVARCHAR(80), Type INT, Color NVARCHAR(7), Icon NVARCHAR(50),
ParentCategoryId GUID NULL, Active BIT, CreatedAt DATETIMEOFFSET, UpdatedAt DATETIMEOFFSET NULL,
IsDeleted BIT).
