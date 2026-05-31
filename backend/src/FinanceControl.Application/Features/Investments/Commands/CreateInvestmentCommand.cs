using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Investments.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Investments.Commands;

public sealed record CreateInvestmentCommand(
    string Ticker, InvestmentType Type, decimal Quantity, decimal AveragePrice,
    string Currency, DateOnly AcquiredAt, Guid BankId,
    decimal? ExpectedYieldPercent, string? Notes) : IRequest<InvestmentDto>;

public sealed class CreateInvestmentValidator : AbstractValidator<CreateInvestmentCommand>
{
    public CreateInvestmentValidator()
    {
        RuleFor(x => x.Ticker).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.AveragePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.BankId).NotEmpty();
    }
}

public sealed class CreateInvestmentHandler : IRequestHandler<CreateInvestmentCommand, InvestmentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateInvestmentHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<InvestmentDto> Handle(CreateInvestmentCommand r, CancellationToken ct)
    {
        var inv = Investment.Create(r.Ticker, r.Type, r.Quantity, r.AveragePrice,
            r.Currency, r.AcquiredAt, r.BankId, r.ExpectedYieldPercent, r.Notes);
        _db.Investments.Add(inv);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<InvestmentDto>(inv);
    }
}

public sealed record RegisterOperationCommand(
    Guid InvestmentId, OperationSide Side, decimal Quantity, decimal Price,
    decimal Fee, DateTimeOffset ExecutedAt) : IRequest<InvestmentDto>;

public sealed class RegisterOperationHandler : IRequestHandler<RegisterOperationCommand, InvestmentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public RegisterOperationHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<InvestmentDto> Handle(RegisterOperationCommand r, CancellationToken ct)
    {
        var inv = await _db.Investments.FirstOrDefaultAsync(i => i.Id == r.InvestmentId, ct)
            ?? throw new NotFoundException("Investment", r.InvestmentId);

        var op = InvestmentOperation.Create(r.InvestmentId, r.Side, r.Quantity, r.Price, r.Fee, r.ExecutedAt);
        inv.ApplyOperation(op);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<InvestmentDto>(inv);
    }
}

public sealed record DeleteInvestmentCommand(Guid Id) : IRequest;

public sealed class DeleteInvestmentHandler : IRequestHandler<DeleteInvestmentCommand>
{
    private readonly IApplicationDbContext _db;
    public DeleteInvestmentHandler(IApplicationDbContext db) => _db = db;

    public async Task Handle(DeleteInvestmentCommand r, CancellationToken ct)
    {
        var inv = await _db.Investments.FirstOrDefaultAsync(i => i.Id == r.Id, ct)
            ?? throw new NotFoundException("Investment", r.Id);
        inv.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }
}
