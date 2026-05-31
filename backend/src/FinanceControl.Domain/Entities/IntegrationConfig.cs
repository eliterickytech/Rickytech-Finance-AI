using FinanceControl.Domain.Common;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

public sealed class IntegrationConfig : BaseEntity
{
    public IntegrationProvider Provider { get; private set; }
    public string ApiKeyEncrypted { get; private set; } = string.Empty;
    public string? ApiSecretEncrypted { get; private set; }
    public bool Active { get; private set; } = true;
    public DateTimeOffset? LastSyncAt { get; private set; }
    public string MetadataJson { get; private set; } = "{}";

    private IntegrationConfig() { }

    public static IntegrationConfig Create(
        IntegrationProvider provider, string apiKeyEncrypted, string? apiSecretEncrypted = null,
        string? metadataJson = null)
    {
        if (string.IsNullOrWhiteSpace(apiKeyEncrypted))
            throw new DomainException("ApiKey é obrigatória.");

        return new IntegrationConfig
        {
            Provider = provider,
            ApiKeyEncrypted = apiKeyEncrypted,
            ApiSecretEncrypted = apiSecretEncrypted,
            MetadataJson = metadataJson ?? "{}"
        };
    }

    public void UpdateCredentials(string apiKeyEncrypted, string? apiSecretEncrypted)
    {
        if (string.IsNullOrWhiteSpace(apiKeyEncrypted))
            throw new DomainException("ApiKey é obrigatória.");
        ApiKeyEncrypted = apiKeyEncrypted;
        ApiSecretEncrypted = apiSecretEncrypted;
        MarkAsUpdated();
    }

    public void TouchSync(DateTimeOffset at)
    {
        LastSyncAt = at;
        MarkAsUpdated();
    }

    public void UpdateMetadata(string json)
    {
        MetadataJson = json ?? "{}";
        MarkAsUpdated();
    }

    public void Activate() { Active = true; MarkAsUpdated(); }
    public void Deactivate() { Active = false; MarkAsUpdated(); }
}
