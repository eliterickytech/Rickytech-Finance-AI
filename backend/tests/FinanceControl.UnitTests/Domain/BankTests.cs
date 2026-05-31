using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace FinanceControl.UnitTests.Domain;

public class BankTests
{
    [Fact]
    public void Create_WithBrlContaCorrente_Succeeds()
    {
        var bank = Bank.Create("Nubank", "Nubank PF", BankAccountType.ContaCorrente, "BRL", 1000m);
        bank.Currency.Should().Be("BRL");
        bank.OpeningBalance.Should().Be(1000m);
    }

    [Fact]
    public void Create_CriptoBankWithBrl_Throws()
    {
        var act = () => Bank.Create("MetaMask", "MetaMask", BankAccountType.Cripto, "BRL", 0m);
        act.Should().Throw<DomainException>().WithMessage("*moeda cripto*");
    }

    [Fact]
    public void Create_NegativeOpeningBalance_Throws()
    {
        var act = () => Bank.Create("Itaú", "Itaú CC", BankAccountType.ContaCorrente, "BRL", -10m);
        act.Should().Throw<DomainException>();
    }
}
