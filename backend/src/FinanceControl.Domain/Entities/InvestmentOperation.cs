using FinanceControl.Domain.Common;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

public sealed class InvestmentOperation : BaseEntity
{
    public Guid InvestmentId { get; private set; }
    public OperationSide Side { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal Fee { get; private set; }
    public DateTimeOffset ExecutedAt { get; private set; }
    public string? IntegrationSourceId { get; private set; }

    private InvestmentOperation() { }

    public static InvestmentOperation Create(
        Guid investmentId, OperationSide side, decimal quantity, decimal price,
        decimal fee, DateTimeOffset executedAt, string? integrationSourceId = null)
    {
        if (quantity <= 0) throw new DomainException("Quantidade da operação deve ser positiva.");
        if (price < 0) throw new DomainException("Preço não pode ser negativo.");
        if (fee < 0) throw new DomainException("Taxa não pode ser negativa.");

        return new InvestmentOperation
        {
            InvestmentId = investmentId,
            Side = side,
            Quantity = quantity,
            Price = price,
            Fee = fee,
            ExecutedAt = executedAt,
            IntegrationSourceId = integrationSourceId
        };
    }
}
