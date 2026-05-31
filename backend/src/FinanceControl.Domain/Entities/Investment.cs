using FinanceControl.Domain.Common;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

public sealed class Investment : BaseEntity
{
    public string Ticker { get; private set; } = string.Empty;
    public InvestmentType Type { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public DateOnly AcquiredAt { get; private set; }
    public Guid BankId { get; private set; }
    public Bank? Bank { get; private set; }
    public decimal? ExpectedYieldPercent { get; private set; }   // anual, para projeções
    public string? Notes { get; private set; }

    private readonly List<InvestmentOperation> _operations = new();
    public IReadOnlyCollection<InvestmentOperation> Operations => _operations;

    private Investment() { }

    public static Investment Create(
        string ticker, InvestmentType type, decimal quantity, decimal averagePrice,
        string currency, DateOnly acquiredAt, Guid bankId,
        decimal? expectedYieldPercent = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(ticker)) throw new DomainException("Ticker obrigatório.");
        if (quantity <= 0) throw new DomainException("Quantidade deve ser positiva.");
        if (averagePrice < 0) throw new DomainException("Preço médio não pode ser negativo.");

        return new Investment
        {
            Ticker = ticker.Trim().ToUpperInvariant(),
            Type = type,
            Quantity = quantity,
            AveragePrice = averagePrice,
            Currency = currency.ToUpperInvariant(),
            AcquiredAt = acquiredAt,
            BankId = bankId,
            ExpectedYieldPercent = expectedYieldPercent,
            Notes = notes?.Trim()
        };
    }

    public void ApplyOperation(InvestmentOperation op)
    {
        if (op.Side == OperationSide.Buy)
        {
            // média ponderada
            var totalCost = (AveragePrice * Quantity) + (op.Price * op.Quantity) + op.Fee;
            var newQty = Quantity + op.Quantity;
            AveragePrice = newQty > 0 ? totalCost / newQty : 0;
            Quantity = newQty;
        }
        else if (op.Side == OperationSide.Sell)
        {
            if (op.Quantity > Quantity) throw new DomainException("Quantidade vendida maior que posição.");
            Quantity -= op.Quantity;
            if (Quantity == 0) AveragePrice = 0;
        }
        // Fee / Yield não alteram Quantity nem AveragePrice diretamente
        _operations.Add(op);
        MarkAsUpdated();
    }

    public void SetNotes(string? notes) { Notes = notes?.Trim(); MarkAsUpdated(); }
    public void SetExpectedYield(decimal? percent) { ExpectedYieldPercent = percent; MarkAsUpdated(); }
}
