using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Features.Investments.Dtos;

public sealed record InvestmentDto(
    Guid Id, string Ticker, InvestmentType Type, decimal Quantity,
    decimal AveragePrice, string Currency, DateOnly AcquiredAt,
    Guid BankId, decimal? ExpectedYieldPercent, string? Notes,
    decimal? CurrentPrice, decimal? CurrentValue, decimal? ProfitLoss, decimal? ProfitLossPercent);

public sealed record CreateInvestmentDto(
    string Ticker, InvestmentType Type, decimal Quantity, decimal AveragePrice,
    string Currency, DateOnly AcquiredAt, Guid BankId,
    decimal? ExpectedYieldPercent, string? Notes);

public sealed record RegisterOperationDto(
    OperationSide Side, decimal Quantity, decimal Price, decimal Fee, DateTimeOffset ExecutedAt);

public sealed record PortfolioSummaryDto(
    decimal TotalInvested, decimal CurrentValue, decimal ProfitLoss, decimal ProfitLossPercent,
    IReadOnlyList<PortfolioByTypeDto> ByType);

public sealed record PortfolioByTypeDto(
    InvestmentType Type, decimal TotalInvested, decimal CurrentValue, decimal ProfitLoss);
