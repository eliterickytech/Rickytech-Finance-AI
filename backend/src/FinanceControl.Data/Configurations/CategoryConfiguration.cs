using FinanceControl.Domain.Entities;
using FinanceControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Data.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> b)
    {
        b.ToTable("Categories");
        b.HasKey(x => x.Id);

        b.Property(x => x.Name).HasMaxLength(80).IsRequired();
        b.Property(x => x.Type).HasConversion<int>().IsRequired();
        b.Property(x => x.Icon).HasMaxLength(50).IsRequired();
        b.Property(x => x.Active).IsRequired();

        b.Property(x => x.Color)
            .HasConversion(c => c.Value, v => HexColor.Create(v))
            .HasMaxLength(7)
            .IsRequired();

        b.HasOne(x => x.Parent)
            .WithMany()
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.Name);
        b.HasIndex(x => x.Type);
    }
}
