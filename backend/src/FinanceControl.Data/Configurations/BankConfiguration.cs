using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Data.Configurations;

public sealed class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> b)
    {
        b.ToTable("Banks");
        b.HasKey(x => x.Id);

        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.Nickname).HasMaxLength(100).IsRequired();
        b.Property(x => x.Type).HasConversion<int>().IsRequired();
        b.Property(x => x.Branch).HasMaxLength(20);
        b.Property(x => x.AccountNumber).HasMaxLength(30);
        b.Property(x => x.Currency).HasMaxLength(10).IsRequired();
        b.Property(x => x.OpeningBalance).HasPrecision(28, 8).IsRequired();
        b.Property(x => x.Active).IsRequired();

        b.HasIndex(x => x.Nickname).IsUnique();
        b.HasIndex(x => x.Type);
    }
}
