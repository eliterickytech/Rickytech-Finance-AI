using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace FinanceControl.UnitTests.Domain;

public class InvestmentTests
{
    [Fact]
    public void ApplyBuyOperation_RecalculatesAveragePrice()
    {
        var inv = Investment.Create("BTC", InvestmentType.Cripto, 1m, 50_000m,
            "USDT", DateOnly.FromDateTime(DateTime.UtcNow), Guid.NewGuid());

        var op = InvestmentOperation.Create(inv.Id, OperationSide.Buy, 1m, 70_000m, 0m, DateTimeOffset.UtcNow);
        inv.ApplyOperation(op);

        inv.Quantity.Should().Be(2m);
        inv.AveragePrice.Should().Be(60_000m); // (50k + 70k) / 2
    }

    [Fact]
    public void ApplySellOperation_ReducesQuantity()
    {
        var inv = Investment.Create("ETH", InvestmentType.Cripto, 10m, 3000m,
            "USDT", DateOnly.FromDateTime(DateTime.UtcNow), Guid.NewGuid());

        var op = InvestmentOperation.Create(inv.Id, OperationSide.Sell, 4m, 3500m, 0m, DateTimeOffset.UtcNow);
        inv.ApplyOperation(op);

        inv.Quantity.Should().Be(6m);
    }
}
