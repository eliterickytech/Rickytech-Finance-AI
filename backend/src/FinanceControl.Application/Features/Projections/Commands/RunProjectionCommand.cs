using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Projections.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Features.Projections.Commands;

public sealed record RunProjectionCommand(
    int HorizonMonths = 12,
    ProjectionScenario Scenario = ProjectionScenario.Realistic,
    decimal InflationPercent = 4.5m,
    bool IncludeInvestments = true,
    Guid[]? BankIds = null) : IRequest<ProjectionResultDto>;

public sealed class RunProjectionValidator : AbstractValidator<RunProjectionCommand>
{
    public RunProjectionValidator()
    {
        RuleFor(x => x.HorizonMonths).InclusiveBetween(1, 60);
        RuleFor(x => x.Scenario).IsInEnum();
        RuleFor(x => x.InflationPercent).InclusiveBetween(-20m, 50m);
    }
}

public sealed class RunProjectionHandler : IRequestHandler<RunProjectionCommand, ProjectionResultDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IDateTime _dateTime;

    public RunProjectionHandler(IApplicationDbContext db, IDateTime dateTime)
    {
        _db = db; _dateTime = dateTime;
    }

    public async Task<ProjectionResultDto> Handle(RunProjectionCommand r, CancellationToken ct)
    {
        var (incomeMult, expenseMult, yieldMult) = GetMultipliers(r.Scenario);

        var bankFilter = r.BankIds ?? Array.Empty<Guid>();
        var banksQ = _db.Banks.AsNoTracking().Where(b => b.Active);
        if (bankFilter.Length > 0) banksQ = banksQ.Where(b => bankFilter.Contains(b.Id));
        var banks = await banksQ.ToListAsync(ct);

        var openingBalance = banks.Sum(b => b.OpeningBalance);

        var incomesQ = _db.Incomes.AsNoTracking();
        var expensesQ = _db.Expenses.AsNoTracking();
        if (bankFilter.Length > 0)
        {
            incomesQ = incomesQ.Where(i => bankFilter.Contains(i.BankId));
            expensesQ = expensesQ.Where(e => bankFilter.Contains(e.BankId));
        }
        var incomes = await incomesQ.ToListAsync(ct);
        var expenses = await expensesQ.ToListAsync(ct);
        var investments = r.IncludeInvestments
            ? await _db.Investments.AsNoTracking().Where(i => i.ExpectedYieldPercent != null).ToListAsync(ct)
            : new List<Investment>();

        var today = _dateTime.Today;
        var inflationMonthly = (decimal)Math.Pow(1d + (double)(r.InflationPercent / 100m), 1d / 12d) - 1m;

        var series = new List<ProjectionMonthDto>(r.HorizonMonths);
        decimal totalIncome = 0, totalExpense = 0, totalYield = 0;
        var rolling = openingBalance;

        for (var m = 1; m <= r.HorizonMonths; m++)
        {
            var monthStart = today.AddMonths(m - 1);
            var monthEnd = today.AddMonths(m).AddDays(-1);

            var incomeMonth = incomes
                .Where(i => OccursInRange(i.Date, i.Recurrence, i.RecurrenceEnd, monthStart, monthEnd))
                .Sum(i => i.Amount) * incomeMult;

            var expenseMonth = expenses
                .Where(e => OccursInRange(e.Date, e.Recurrence, e.RecurrenceEnd, monthStart, monthEnd))
                .Sum(e => e.Amount) * expenseMult * (decimal)Math.Pow(1d + (double)inflationMonthly, m - 1);

            var yieldMonth = investments
                .Sum(i => (i.AveragePrice * i.Quantity) * ((i.ExpectedYieldPercent ?? 0) / 100m / 12m))
                * yieldMult;

            var ending = rolling + incomeMonth - expenseMonth + yieldMonth;

            series.Add(new ProjectionMonthDto(
                $"{monthStart:yyyy-MM}",
                Math.Round(rolling, 2),
                Math.Round(incomeMonth, 2),
                Math.Round(expenseMonth, 2),
                Math.Round(yieldMonth, 2),
                Math.Round(ending, 2)));

            totalIncome += incomeMonth;
            totalExpense += expenseMonth;
            totalYield += yieldMonth;
            rolling = ending;
        }

        var summary = new ProjectionSummaryDto(
            Math.Round(rolling, 2),
            Math.Round(totalIncome, 2),
            Math.Round(totalExpense, 2),
            Math.Round(totalYield, 2),
            Math.Round(totalIncome - totalExpense + totalYield, 2));

        return new ProjectionResultDto(r.HorizonMonths, r.Scenario, _dateTime.UtcNow, series, summary);
    }

    private static (decimal income, decimal expense, decimal yield) GetMultipliers(ProjectionScenario s) => s switch
    {
        ProjectionScenario.Optimistic => (1.05m, 0.95m, 1.20m),
        ProjectionScenario.Pessimistic => (0.95m, 1.10m, 0.80m),
        _ => (1.00m, 1.00m, 1.00m)
    };

    private static bool OccursInRange(DateOnly start, RecurrenceFrequency freq, DateOnly? end, DateOnly rangeStart, DateOnly rangeEnd)
    {
        if (freq == RecurrenceFrequency.Once)
            return start >= rangeStart && start <= rangeEnd;

        var hardEnd = end ?? DateOnly.MaxValue;
        var d = start;
        while (d <= rangeEnd && d <= hardEnd)
        {
            if (d >= rangeStart && d <= rangeEnd) return true;
            d = freq switch
            {
                RecurrenceFrequency.Daily => d.AddDays(1),
                RecurrenceFrequency.Weekly => d.AddDays(7),
                RecurrenceFrequency.BiWeekly => d.AddDays(14),
                RecurrenceFrequency.Monthly => d.AddMonths(1),
                RecurrenceFrequency.Quarterly => d.AddMonths(3),
                RecurrenceFrequency.SemiAnnual => d.AddMonths(6),
                RecurrenceFrequency.Annual => d.AddYears(1),
                _ => DateOnly.MaxValue
            };
        }
        return false;
    }
}
