using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Features.Integrations.Dtos;

public sealed record IntegrationStatusDto(
    Guid? Id, IntegrationProvider Provider, bool Connected, bool Active,
    DateTimeOffset? LastSyncAt, string? ApiKeyMasked);

public sealed record SaveBinanceCredentialsDto(string ApiKey, string ApiSecret);

public sealed record SyncResultDto(int Imported, int Skipped, DateTimeOffset SyncedAt);

public sealed record StartConsentDto(string Cpf, string BankCode);

public sealed record ConsentUrlDto(string ConsentUrl, string ConsentId);
