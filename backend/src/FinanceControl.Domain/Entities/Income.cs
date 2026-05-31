using FinanceControl.Domain.Enums;

namespace FinanceControl.Domain.Entities;

public sealed class Income : FinancialTransaction
{
    private Income() { }

    public static Income Create(
        string description, decimal amount, DateOnly date, Guid categoryId, Guid bankId,
        string[]? tags = null, RecurrenceFrequency recurrence = RecurrenceFrequency.Once,
        DateOnly? recurrenceEnd = null, string? integrationSourceId = null, bool confirmed = true)
    {
        var income = new Income();
        income.Init(description, amount, date, categoryId, bankId, tags, recurrence, recurrenceEnd, integrationSourceId, confirmed);
        return income;
    }
}
