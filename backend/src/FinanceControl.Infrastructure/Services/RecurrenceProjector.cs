using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Domain.Enums;

namespace FinanceControl.Infrastructure.Services;

public sealed class RecurrenceProjector : IRecurrenceProjector
{
    public IEnumerable<DateOnly> Project(DateOnly start, RecurrenceFrequency frequency, DateOnly? end, int maxOccurrences = 24)
    {
        if (frequency == RecurrenceFrequency.Once)
        {
            yield return start;
            yield break;
        }

        var hardLimit = end ?? start.AddMonths(24);
        var current = start;
        var count = 0;

        while (current <= hardLimit && count < maxOccurrences)
        {
            yield return current;
            count++;
            current = Next(current, frequency);
        }
    }

    private static DateOnly Next(DateOnly d, RecurrenceFrequency f) => f switch
    {
        RecurrenceFrequency.Daily      => d.AddDays(1),
        RecurrenceFrequency.Weekly     => d.AddDays(7),
        RecurrenceFrequency.BiWeekly   => d.AddDays(14),
        RecurrenceFrequency.Monthly    => d.AddMonths(1),
        RecurrenceFrequency.Quarterly  => d.AddMonths(3),
        RecurrenceFrequency.SemiAnnual => d.AddMonths(6),
        RecurrenceFrequency.Annual     => d.AddYears(1),
        _ => d.AddYears(100)
    };
}
