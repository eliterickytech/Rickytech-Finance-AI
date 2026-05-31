using AutoMapper;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Banks.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;

namespace FinanceControl.Application.Features.Banks.Commands.CreateBank;

public sealed record CreateBankCommand(
    string Name, string Nickname, BankAccountType Type, string Currency,
    decimal OpeningBalance, string? Branch, string? AccountNumber) : IRequest<BankDto>;

public sealed class CreateBankValidator : AbstractValidator<CreateBankCommand>
{
    public CreateBankValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Nickname).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.OpeningBalance).GreaterThanOrEqualTo(0);
    }
}

public sealed class CreateBankHandler : IRequestHandler<CreateBankCommand, BankDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateBankHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<BankDto> Handle(CreateBankCommand r, CancellationToken ct)
    {
        var bank = Bank.Create(r.Name, r.Nickname, r.Type, r.Currency, r.OpeningBalance, r.Branch, r.AccountNumber);
        _db.Banks.Add(bank);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<BankDto>(bank);
    }
}
