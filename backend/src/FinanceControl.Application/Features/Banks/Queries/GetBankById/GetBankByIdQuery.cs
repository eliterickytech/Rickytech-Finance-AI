using AutoMapper;
using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Banks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Banks.Queries.GetBankById;

public sealed record GetBankByIdQuery(Guid Id) : IRequest<BankDto>;

public sealed class GetBankByIdHandler : IRequestHandler<GetBankByIdQuery, BankDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetBankByIdHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<BankDto> Handle(GetBankByIdQuery r, CancellationToken ct)
    {
        var bank = await _db.Banks.AsNoTracking().FirstOrDefaultAsync(b => b.Id == r.Id, ct)
            ?? throw new NotFoundException("Bank", r.Id);
        return _mapper.Map<BankDto>(bank);
    }
}
