using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Data.Configurations;

public sealed class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> b)
    {
        b.ToTable("Incomes");
        ConfigureBase(b);
    }

    internal static void ConfigureBase<T>(EntityTypeBuilder<T> b) where T : FinancialTransaction
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Description).HasMaxLength(200).IsRequired();
        b.Property(x => x.Amount).HasPrecision(28, 8).IsRequired();
        b.Property(x => x.Date).IsRequired();
        b.Property(x => x.Recurrence).HasConversion<int>().IsRequired();
        b.Property(x => x.IntegrationSourceId).HasMaxLength(150);
        b.Property(x => x.AttachmentPath).HasMaxLength(500);
        b.Property(x => x.Confirmed).IsRequired();

        b.Property(x => x.Tags)
            .HasConversion(
                v => string.Join('|', v),
                v => string.IsNullOrEmpty(v) ? Array.Empty<string>() : v.Split('|', StringSplitOptions.RemoveEmptyEntries))
            .HasMaxLength(500);

        b.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Bank).WithMany().HasForeignKey(x => x.BankId).OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.BankId, x.Date });
        b.HasIndex(x => x.CategoryId);
        b.HasIndex(x => x.IntegrationSourceId);
    }
}

public sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> b)
    {
        b.ToTable("Expenses");
        IncomeConfiguration.ConfigureBase(b);
        b.Property(x => x.PaymentMethod).HasConversion<int>().IsRequired();
    }
}
