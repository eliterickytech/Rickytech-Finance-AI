using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Incomes.Commands.DeleteIncome;

public sealed record DeleteIncomeCommand(Guid Id) : IRequest;

public sealed class DeleteIncomeHandler : IRequestHandler<DeleteIncomeCommand>
{
    private readonly IApplicationDbContext _db;
    public DeleteIncomeHandler(IApplicationDbContext db) => _db = db;

    public async Task Handle(DeleteIncomeCommand r, CancellationToken ct)
    {
        var income = await _db.Incomes.FirstOrDefaultAsync(i => i.Id == r.Id, ct)
            ?? throw new NotFoundException("Income", r.Id);
        income.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }
}
