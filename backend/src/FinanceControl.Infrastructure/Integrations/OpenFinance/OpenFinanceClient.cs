using System.Runtime.CompilerServices;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.CrossCutting.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinanceControl.Infrastructure.Integrations.OpenFinance;

/// <summary>
/// Implementação Mock/Sandbox do Open Finance Brasil.
/// Em "Mock" devolve dados sintéticos; em "Sandbox" chama HTTPs reais (TODO).
/// </summary>
public sealed class OpenFinanceClient : IOpenFinanceClient
{
    private readonly HttpClient _http;
    private readonly OpenFinanceOptions _options;
    private readonly ILogger<OpenFinanceClient> _logger;

    public OpenFinanceClient(HttpClient http, IOptions<IntegrationsOptions> options, ILogger<OpenFinanceClient> logger)
    {
        _http = http;
        _options = options.Value.OpenFinance;
        _logger = logger;
        if (!string.IsNullOrWhiteSpace(_options.BaseUrl))
            _http.BaseAddress = new Uri(_options.BaseUrl);
    }

    public Task<OfiConsent> CreateConsentAsync(string cpf, string bankCode, CancellationToken ct)
    {
        var consentId = $"consent-{Guid.NewGuid():N}";
        var url = $"https://mock.openfinance.local/consent/{consentId}?cpf={cpf}&bank={bankCode}";
        _logger.LogInformation("Criando consent {ConsentId} para CPF mascarado e banco {Bank}", consentId, bankCode);
        return Task.FromResult(new OfiConsent(consentId, url));
    }

    public Task<OfiToken> ExchangeCodeAsync(string code, string state, CancellationToken ct)
        => Task.FromResult(new OfiToken(
            $"access-{Guid.NewGuid():N}",
            $"refresh-{Guid.NewGuid():N}",
            DateTimeOffset.UtcNow.AddHours(1)));

    public Task<OfiToken> RefreshAsync(string refreshToken, CancellationToken ct)
        => Task.FromResult(new OfiToken(
            $"access-{Guid.NewGuid():N}",
            refreshToken,
            DateTimeOffset.UtcNow.AddHours(1)));

    public async IAsyncEnumerable<OfiAccount> GetAccountsAsync(
        string accessToken, [EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();
        yield return new OfiAccount("acc-001", "Conta Corrente", "CACC", "BRL");
        yield return new OfiAccount("acc-002", "Poupança", "SVGS", "BRL");
    }

    public async IAsyncEnumerable<OfiTransaction> GetTransactionsAsync(
        string accessToken, string accountId, DateOnly since, [EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();
        var samples = new (string Type, string Desc, decimal Amount)[]
        {
            ("CREDIT", "PIX RECEBIDO - JOAO", 250.00m),
            ("CREDIT", "SALARIO FOLHA",      6500.00m),
            ("DEBIT",  "SUPERMERCADO EXTRA",  187.45m),
            ("DEBIT",  "POSTO IPIRANGA",      210.00m),
            ("DEBIT",  "NETFLIX MENSALIDADE",  39.90m),
            ("DEBIT",  "BOLETO ENERGIA",      189.00m)
        };

        foreach (var (type, desc, amount) in samples)
        {
            yield return new OfiTransaction(
                Guid.NewGuid().ToString("N"), accountId, amount, type,
                DateOnly.FromDateTime(DateTime.UtcNow), desc, null);
        }
    }
}

public sealed class KeywordTransactionClassifier : ITransactionClassifier
{
    private static readonly (string Keyword, string CategoryName)[] Rules =
    {
        ("PIX RECEBIDO", "Sem categoria"),
        ("SALARIO",      "Salário"),
        ("FOLHA",        "Salário"),
        ("SUPERMERCADO", "Alimentação"),
        ("MERCADO",      "Alimentação"),
        ("EXTRA",        "Alimentação"),
        ("POSTO",        "Transporte"),
        ("COMBUSTIVEL",  "Transporte"),
        ("UBER",         "Transporte"),
        ("NETFLIX",      "Assinaturas"),
        ("SPOTIFY",      "Assinaturas"),
        ("BOLETO",       "Moradia"),
        ("ENERGIA",      "Moradia"),
        ("BINANCE",      "Depósito Cripto"),
        ("MERCADO BITCOIN", "Depósito Cripto")
    };

    public Guid Classify(string description, bool isCredit, IReadOnlyDictionary<string, Guid> categoryIndex)
    {
        var upper = description.ToUpperInvariant();
        foreach (var (kw, name) in Rules)
        {
            if (upper.Contains(kw) && categoryIndex.TryGetValue(name, out var id))
                return id;
        }
        return categoryIndex.TryGetValue("Sem categoria", out var fallback) ? fallback : Guid.Empty;
    }
}
