using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Integrations.Dtos;
using FinanceControl.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Integrations.Queries;

public sealed record GetBinanceStatusQuery : IRequest<IntegrationStatusDto>;

public sealed class GetBinanceStatusHandler : IRequestHandler<GetBinanceStatusQuery, IntegrationStatusDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICryptoCipher _cipher;
    public GetBinanceStatusHandler(IApplicationDbContext db, ICryptoCipher cipher) { _db = db; _cipher = cipher; }

    public async Task<IntegrationStatusDto> Handle(GetBinanceStatusQuery r, CancellationToken ct)
    {
        var cfg = await _db.IntegrationConfigs.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Provider == IntegrationProvider.Binance, ct);

        if (cfg is null)
            return new IntegrationStatusDto(null, IntegrationProvider.Binance, false, false, null, null);

        var apiKey = _cipher.Decrypt(cfg.ApiKeyEncrypted);
        var masked = apiKey.Length <= 8 ? "****" : $"{apiKey[..4]}****{apiKey[^4..]}";

        return new IntegrationStatusDto(cfg.Id, IntegrationProvider.Binance, true, cfg.Active, cfg.LastSyncAt, masked);
    }
}
