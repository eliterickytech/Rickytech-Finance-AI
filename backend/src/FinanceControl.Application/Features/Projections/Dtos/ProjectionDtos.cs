namespace FinanceControl.Application.Features.Projections.Dtos;

public enum ProjectionScenario
{
    Optimistic = 1,
    Realistic = 2,
    Pessimistic = 3
}

public sealed record ProjectionMonthDto(
    string Month,
    decimal OpeningBalance,
    decimal Income,
    decimal Expense,
    decimal InvestmentYield,
    decimal EndingBalance);

public sealed record ProjectionSummaryDto(
    decimal EndingBalance,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal TotalInvestmentYield,
    decimal NetProfit);

public sealed record ProjectionResultDto(
    int HorizonMonths,
    ProjectionScenario Scenario,
    DateTimeOffset GeneratedAt,
    IReadOnlyList<ProjectionMonthDto> Series,
    ProjectionSummaryDto Summary);
