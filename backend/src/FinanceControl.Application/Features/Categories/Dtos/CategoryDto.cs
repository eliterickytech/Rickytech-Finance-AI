using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Features.Categories.Dtos;

public sealed record CategoryDto(
    Guid Id,
    string Name,
    CategoryType Type,
    string Color,
    string Icon,
    Guid? ParentCategoryId,
    bool Active);

public sealed record CreateCategoryDto(
    string Name,
    CategoryType Type,
    string Color,
    string Icon,
    Guid? ParentCategoryId);

public sealed record UpdateCategoryDto(
    string Name,
    CategoryType Type,
    string Color,
    string Icon,
    bool Active);
