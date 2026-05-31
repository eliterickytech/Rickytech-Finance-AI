using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Data.Configurations;

public sealed class InvestmentConfiguration : IEntityTypeConfiguration<Investment>
{
    public void Configure(EntityTypeBuilder<Investment> b)
    {
        b.ToTable("Investments");
        b.HasKey(x => x.Id);

        b.Property(x => x.Ticker).HasMaxLength(30).IsRequired();
        b.Property(x => x.Type).HasConversion<int>().IsRequired();
        b.Property(x => x.Quantity).HasPrecision(28, 18).IsRequired();
        b.Property(x => x.AveragePrice).HasPrecision(28, 8).IsRequired();
        b.Property(x => x.Currency).HasMaxLength(10).IsRequired();
        b.Property(x => x.AcquiredAt).IsRequired();
        b.Property(x => x.ExpectedYieldPercent).HasPrecision(10, 4);
        b.Property(x => x.Notes).HasMaxLength(1000);

        b.HasOne(x => x.Bank).WithMany().HasForeignKey(x => x.BankId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(x => x.Operations).WithOne().HasForeignKey(o => o.InvestmentId).OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.Ticker);
        b.HasIndex(x => x.Type);
    }
}

public sealed class InvestmentOperationConfiguration : IEntityTypeConfiguration<InvestmentOperation>
{
    public void Configure(EntityTypeBuilder<InvestmentOperation> b)
    {
        b.ToTable("InvestmentOperations");
        b.HasKey(x => x.Id);

        b.Property(x => x.Side).HasConversion<int>().IsRequired();
        b.Property(x => x.Quantity).HasPrecision(28, 18).IsRequired();
        b.Property(x => x.Price).HasPrecision(28, 8).IsRequired();
        b.Property(x => x.Fee).HasPrecision(28, 8).IsRequired();
        b.Property(x => x.IntegrationSourceId).HasMaxLength(150);

        b.HasIndex(x => x.InvestmentId);
        b.HasIndex(x => x.IntegrationSourceId);
    }
}

public sealed class AssetQuoteConfiguration : IEntityTypeConfiguration<AssetQuote>
{
    public void Configure(EntityTypeBuilder<AssetQuote> b)
    {
        b.ToTable("AssetQuotes");
        b.HasKey(x => x.Id);

        b.Property(x => x.Ticker).HasMaxLength(30).IsRequired();
        b.Property(x => x.Price).HasPrecision(28, 8).IsRequired();
        b.Property(x => x.Currency).HasMaxLength(10).IsRequired();
        b.Property(x => x.Source).HasMaxLength(50).IsRequired();

        b.HasIndex(x => new { x.Ticker, x.Date, x.Currency }).IsUnique();
    }
}
