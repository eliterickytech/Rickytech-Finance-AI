using FinanceControl.Domain.Common;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;
using FinanceControl.Domain.ValueObjects;

namespace FinanceControl.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; }
    public HexColor Color { get; private set; } = HexColor.Default;
    public string Icon { get; private set; } = "fa-folder";
    public Guid? ParentCategoryId { get; private set; }
    public Category? Parent { get; private set; }
    public bool Active { get; private set; } = true;

    private Category() { }   // EF Core

    public static Category Create(string name, CategoryType type, HexColor color, string icon, Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome da categoria é obrigatório.");
        if (name.Length > 80)
            throw new DomainException("Nome da categoria deve ter no máximo 80 caracteres.");
        if (string.IsNullOrWhiteSpace(icon))
            throw new DomainException("Ícone da categoria é obrigatório.");

        return new Category
        {
            Name = name.Trim(),
            Type = type,
            Color = color,
            Icon = icon.Trim(),
            ParentCategoryId = parentCategoryId
        };
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Nome da categoria é obrigatório.");
        Name = newName.Trim();
        MarkAsUpdated();
    }

    public void ChangeType(CategoryType newType)
    {
        Type = newType;
        MarkAsUpdated();
    }

    public void ChangeAppearance(HexColor color, string icon)
    {
        if (string.IsNullOrWhiteSpace(icon))
            throw new DomainException("Ícone da categoria é obrigatório.");
        Color = color;
        Icon = icon.Trim();
        MarkAsUpdated();
    }

    public void Activate() { Active = true; MarkAsUpdated(); }
    public void Deactivate() { Active = false; MarkAsUpdated(); }
}
