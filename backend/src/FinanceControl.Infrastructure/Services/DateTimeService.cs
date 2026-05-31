using FinanceControl.Application.Common.Interfaces;

namespace FinanceControl.Infrastructure.Services;

public sealed class DateTimeService : IDateTime
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
}
