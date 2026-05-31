using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.ValueObjects;

/// <summary>
/// Value Object monetário multi-moeda. Currency aceita ISO 4217 (BRL, USD)
/// e tickers cripto (BTC, ETH, USDT, ADA, SOL, BNB).
/// </summary>
public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Moeda é obrigatória.");
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency) => new(0m, currency);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException($"Não é possível operar moedas diferentes: {Currency} vs {other.Currency}.");
    }

    public override string ToString() => $"{Amount:N2} {Currency}";
}
