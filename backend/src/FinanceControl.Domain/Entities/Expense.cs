using FinanceControl.Domain.Enums;

namespace FinanceControl.Domain.Entities;

public sealed class Expense : FinancialTransaction
{
    public PaymentMethod PaymentMethod { get; private set; } = PaymentMethod.Other;

    private Expense() { }

    public static Expense Create(
        string description, decimal amount, DateOnly date, Guid categoryId, Guid bankId,
        PaymentMethod paymentMethod, string[]? tags = null,
        RecurrenceFrequency recurrence = RecurrenceFrequency.Once,
        DateOnly? recurrenceEnd = null, string? integrationSourceId = null, bool confirmed = true)
    {
        var expense = new Expense { PaymentMethod = paymentMethod };
        expense.Init(description, amount, date, categoryId, bankId, tags, recurrence, recurrenceEnd, integrationSourceId, confirmed);
        return expense;
    }

    public void ChangePaymentMethod(PaymentMethod method)
    {
        PaymentMethod = method;
        MarkAsUpdated();
    }
}
