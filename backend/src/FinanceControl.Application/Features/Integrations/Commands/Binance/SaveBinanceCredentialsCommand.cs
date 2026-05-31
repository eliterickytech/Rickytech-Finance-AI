using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Integrations.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Integrations.Commands.Binance;

public sealed record SaveBinanceCredentialsCommand(string ApiKey, string ApiSecret) : IRequest<IntegrationStatusDto>;

public sealed class SaveBinanceCredentialsValidator : AbstractValidator<SaveBinanceCredentialsCommand>
{
    public SaveBinanceCredentialsValidator()
    {
        RuleFor(x => x.ApiKey).NotEmpty().MinimumLength(16);
        RuleFor(x => x.ApiSecret).NotEmpty().MinimumLength(16);
    }
}

public sealed class SaveBinanceCredentialsHandler : IRequestHandler<SaveBinanceCredentialsCommand, IntegrationStatusDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICryptoCipher _cipher;
    private readonly IBinanceClient _binance;
    private readonly IDateTime _dateTime;

    public SaveBinanceCredentialsHandler(
        IApplicationDbContext db, ICryptoCipher cipher, IBinanceClient binance, IDateTime dateTime)
    {
        _db = db; _cipher = cipher; _binance = binance; _dateTime = dateTime;
    }

    public async Task<IntegrationStatusDto> Handle(SaveBinanceCredentialsCommand r, CancellationToken ct)
    {
        var valid = await _binance.TestCredentialsAsync(r.ApiKey, r.ApiSecret, ct);
        if (!valid) throw new UnauthorizedException("Credenciais inválidas na Binance.");

        var cfg = await _db.IntegrationConfigs.FirstOrDefaultAsync(c => c.Provider == IntegrationProvider.Binance, ct);

        var keyEnc = _cipher.Encrypt(r.ApiKey);
        var secretEnc = _cipher.Encrypt(r.ApiSecret);

        if (cfg is null)
        {
            cfg = IntegrationConfig.Create(IntegrationProvider.Binance, keyEnc, secretEnc);
            _db.IntegrationConfigs.Add(cfg);
        }
        else
        {
            cfg.UpdateCredentials(keyEnc, secretEnc);
            cfg.Activate();
        }

        await _db.SaveChangesAsync(ct);

        return new IntegrationStatusDto(
            cfg.Id, IntegrationProvider.Binance, true, cfg.Active, cfg.LastSyncAt, MaskKey(r.ApiKey));
    }

    private static string MaskKey(string key)
        => key.Length <= 8 ? "****" : $"{key[..4]}****{key[^4..]}";
}
