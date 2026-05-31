namespace FinanceControl.CrossCutting.Configurations;

/// <summary>
/// Bind fortemente tipado da seção "Integrations" do appsettings.json.
/// </summary>
public sealed class IntegrationsOptions
{
    public const string SectionName = "Integrations";

    public BinanceOptions Binance { get; set; } = new();
    public OpenFinanceOptions OpenFinance { get; set; } = new();
    public NewsOptions News { get; set; } = new();
}

public sealed class BinanceOptions
{
    public string BaseUrl { get; set; } = "https://api.binance.com";
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
    public int SyncWindowDays { get; set; } = 90;
}

public sealed class OpenFinanceOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Mode { get; set; } = "Mock";   // Mock | Sandbox | Production
}

public sealed class NewsOptions
{
    public List<NewsSource> Sources { get; set; } = new();
    public int RefreshIntervalMinutes { get; set; } = 15;
    public int RetentionDays { get; set; } = 7;
    public Dictionary<string, string[]> TagDictionary { get; set; } = new();
}

public sealed class NewsSource
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Category { get; set; } = "All";   // Crypto | FinancialBR | FinancialIntl
}

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public string Provider { get; set; } = "SqlServer";   // SqlServer | Sqlite
}
