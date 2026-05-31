using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Banks.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Banks.Commands.UpdateBank;

public sealed record UpdateBankCommand(Guid Id, string Nickname, bool Active) : IRequest<BankDto>;

public sealed class UpdateBankValidator : AbstractValidator<UpdateBankCommand>
{
    public UpdateBankValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nickname).NotEmpty().MaximumLength(100);
    }
}

public sealed class UpdateBankHandler : IRequestHandler<UpdateBankCommand, BankDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public UpdateBankHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<BankDto> Handle(UpdateBankCommand r, CancellationToken ct)
    {
        var bank = await _db.Banks.FirstOrDefaultAsync(b => b.Id == r.Id, ct)
            ?? throw new NotFoundException("Bank", r.Id);

        bank.Rename(r.Nickname);
        if (r.Active) bank.Activate(); else bank.Deactivate();
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<BankDto>(bank);
    }
}
