using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;
using FinanceControl.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace FinanceControl.UnitTests.Domain;

public class CategoryTests
{
    [Fact]
    public void Create_WithValidData_Succeeds()
    {
        var category = Category.Create("Alimentação", CategoryType.Expense,
            HexColor.Create("#FF5733"), "fa-utensils");

        category.Name.Should().Be("Alimentação");
        category.Type.Should().Be(CategoryType.Expense);
        category.Color.Value.Should().Be("#FF5733");
        category.Active.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyName_Throws(string name)
    {
        var act = () => Category.Create(name, CategoryType.Expense, HexColor.Default, "fa-folder");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Deactivate_SetsActiveFalse()
    {
        var category = Category.Create("Lazer", CategoryType.Expense, HexColor.Default, "fa-gamepad");
        category.Deactivate();
        category.Active.Should().BeFalse();
    }
}

public class HexColorTests
{
    [Theory]
    [InlineData("#FFFFFF")]
    [InlineData("#000000")]
    [InlineData("#348fe2")]
    public void Create_WithValidHex_Succeeds(string hex)
    {
        var color = HexColor.Create(hex);
        color.Value.Should().Be(hex.ToUpperInvariant());
    }

    [Theory]
    [InlineData("348FE2")]   // sem #
    [InlineData("#XYZ")]
    [InlineData("#12345")]
    public void Create_WithInvalidHex_Throws(string hex)
    {
        var act = () => HexColor.Create(hex);
        act.Should().Throw<ArgumentException>();
    }
}
