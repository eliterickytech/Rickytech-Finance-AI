using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Categories.Dtos;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid Id,
    string Name,
    CategoryType Type,
    string Color,
    string Icon,
    bool Active) : IRequest<CategoryDto>;

public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Color).Matches("^#([0-9A-Fa-f]{6})$");
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(50);
    }
}

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdateCategoryHandler(IApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(FinanceControl.Domain.Entities.Category), request.Id);

        category.Rename(request.Name);
        category.ChangeType(request.Type);
        category.ChangeAppearance(HexColor.Create(request.Color), request.Icon);
        if (request.Active) category.Activate(); else category.Deactivate();

        await _db.SaveChangesAsync(ct);
        return _mapper.Map<CategoryDto>(category);
    }
}
