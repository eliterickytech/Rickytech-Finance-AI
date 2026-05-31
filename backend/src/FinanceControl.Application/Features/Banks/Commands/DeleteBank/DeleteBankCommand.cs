using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Banks.Commands.DeleteBank;

public sealed record DeleteBankCommand(Guid Id) : IRequest;

public sealed class DeleteBankHandler : IRequestHandler<DeleteBankCommand>
{
    private readonly IApplicationDbContext _db;
    public DeleteBankHandler(IApplicationDbContext db) => _db = db;

    public async Task Handle(DeleteBankCommand r, CancellationToken ct)
    {
        var bank = await _db.Banks.FirstOrDefaultAsync(b => b.Id == r.Id, ct)
            ?? throw new NotFoundException("Bank", r.Id);
        bank.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }
}
