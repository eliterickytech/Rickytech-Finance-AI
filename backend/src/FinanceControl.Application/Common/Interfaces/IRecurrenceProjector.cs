using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Common.Interfaces;

/// <summary>
/// Projeta as próximas ocorrências de uma transação recorrente.
/// </summary>
public interface IRecurrenceProjector
{
    IEnumerable<DateOnly> Project(DateOnly start, RecurrenceFrequency frequency, DateOnly? end, int maxOccurrences = 24);
}
