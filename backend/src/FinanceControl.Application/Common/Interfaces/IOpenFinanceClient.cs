namespace FinanceControl.Application.Common.Interfaces;

public sealed record OfiConsent(string ConsentId, string ConsentUrl);
public sealed record OfiToken(string AccessToken, string RefreshToken, DateTimeOffset ExpiresAt);
public sealed record OfiAccount(string AccountId, string Name, string Type, string Currency);
public sealed record OfiTransaction(
    string TransactionId, string AccountId, decimal Amount, string CreditDebitType,
    DateOnly Date, string Description, string? Category);

public interface IOpenFinanceClient
{
    Task<OfiConsent> CreateConsentAsync(string cpf, string bankCode, CancellationToken ct);
    Task<OfiToken> ExchangeCodeAsync(string code, string state, CancellationToken ct);
    Task<OfiToken> RefreshAsync(string refreshToken, CancellationToken ct);
    IAsyncEnumerable<OfiAccount> GetAccountsAsync(string accessToken, CancellationToken ct);
    IAsyncEnumerable<OfiTransaction> GetTransactionsAsync(string accessToken, string accountId, DateOnly since, CancellationToken ct);
}

public interface ITransactionClassifier
{
    Guid Classify(string description, bool isCredit, IReadOnlyDictionary<string, Guid> categoryIndex);
}
