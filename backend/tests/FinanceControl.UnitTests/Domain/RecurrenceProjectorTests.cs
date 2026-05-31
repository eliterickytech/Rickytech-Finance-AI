using FinanceControl.Domain.Enums;
using FinanceControl.Infrastructure.Services;
using FluentAssertions;
using Xunit;

namespace FinanceControl.UnitTests.Domain;

public class RecurrenceProjectorTests
{
    private readonly RecurrenceProjector _projector = new();

    [Fact]
    public void Project_Once_ReturnsSingleDate()
    {
        var dates = _projector.Project(new DateOnly(2026, 1, 15), RecurrenceFrequency.Once, null).ToList();
        dates.Should().ContainSingle().Which.Should().Be(new DateOnly(2026, 1, 15));
    }

    [Fact]
    public void Project_Monthly_Returns12Occurrences()
    {
        var start = new DateOnly(2026, 1, 1);
        var dates = _projector.Project(start, RecurrenceFrequency.Monthly, null, 12).ToList();
        dates.Should().HaveCount(12);
        dates.Last().Should().Be(new DateOnly(2026, 12, 1));
    }

    [Fact]
    public void Project_RespectsEndDate()
    {
        var start = new DateOnly(2026, 1, 1);
        var end = new DateOnly(2026, 3, 31);
        var dates = _projector.Project(start, RecurrenceFrequency.Monthly, end).ToList();
        dates.Should().HaveCount(3);
    }
}
