using FinanceControl.Domain.Common;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

public sealed class AssetQuote : BaseEntity
{
    public string Ticker { get; private set; } = string.Empty;
    public DateOnly Date { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "USD";
    public string Source { get; private set; } = "Mock";

    private AssetQuote() { }

    public static AssetQuote Create(string ticker, DateOnly date, decimal price, string currency, string source)
    {
        if (string.IsNullOrWhiteSpace(ticker)) throw new DomainException("Ticker obrigatório.");
        if (price < 0) throw new DomainException("Preço não pode ser negativo.");

        return new AssetQuote
        {
            Ticker = ticker.ToUpperInvariant(),
            Date = date,
            Price = price,
            Currency = currency.ToUpperInvariant(),
            Source = source
        };
    }
}
