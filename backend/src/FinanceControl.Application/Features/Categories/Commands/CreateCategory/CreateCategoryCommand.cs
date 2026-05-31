using AutoMapper;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Categories.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace FinanceControl.Application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name,
    CategoryType Type,
    string Color,
    string Icon,
    Guid? ParentCategoryId) : IRequest<CategoryDto>;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Color).Matches("^#([0-9A-Fa-f]{6})$").WithMessage("Cor deve estar no formato #RRGGBB.");
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(50);
    }
}

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var category = Category.Create(
            request.Name,
            request.Type,
            HexColor.Create(request.Color),
            request.Icon,
            request.ParentCategoryId);

        _db.Categories.Add(category);
        await _db.SaveChangesAsync(ct);

        return _mapper.Map<CategoryDto>(category);
    }
}
