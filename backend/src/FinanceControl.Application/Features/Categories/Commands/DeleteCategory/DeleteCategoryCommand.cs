using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest;

public sealed class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryValidator() => RuleFor(x => x.Id).NotEmpty();
}

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _db;
    public DeleteCategoryHandler(IApplicationDbContext db) => _db = db;

    public async Task Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, ct)
            ?? throw new NotFoundException("Category", request.Id);

        category.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }
}
