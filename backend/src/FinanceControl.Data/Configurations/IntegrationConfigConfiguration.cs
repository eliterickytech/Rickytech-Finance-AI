using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceControl.Data.Configurations;

public sealed class IntegrationConfigConfiguration : IEntityTypeConfiguration<IntegrationConfig>
{
    public void Configure(EntityTypeBuilder<IntegrationConfig> b)
    {
        b.ToTable("IntegrationConfigs");
        b.HasKey(x => x.Id);

        b.Property(x => x.Provider).HasConversion<int>().IsRequired();
        b.Property(x => x.ApiKeyEncrypted).HasMaxLength(2000).IsRequired();
        b.Property(x => x.ApiSecretEncrypted).HasMaxLength(2000);
        b.Property(x => x.MetadataJson).HasMaxLength(4000).IsRequired();

        b.HasIndex(x => x.Provider);
    }
}

public sealed class NewsItemConfiguration : IEntityTypeConfiguration<NewsItem>
{
    public void Configure(EntityTypeBuilder<NewsItem> b)
    {
        b.ToTable("NewsItems");
        b.HasKey(x => x.Id);

        b.Property(x => x.Title).HasMaxLength(500).IsRequired();
        b.Property(x => x.Url).HasMaxLength(1000).IsRequired();
        b.Property(x => x.UrlHash).HasMaxLength(80).IsRequired();
        b.Property(x => x.Source).HasMaxLength(100).IsRequired();
        b.Property(x => x.Category).HasConversion<int>().IsRequired();
        b.Property(x => x.Summary).HasMaxLength(2000);
        b.Property(x => x.ImageUrl).HasMaxLength(1000);

        b.Property(x => x.Tags)
            .HasConversion(
                v => string.Join('|', v),
                v => string.IsNullOrEmpty(v) ? Array.Empty<string>() : v.Split('|', StringSplitOptions.RemoveEmptyEntries))
            .HasMaxLength(500);

        b.HasIndex(x => x.UrlHash).IsUnique();
        b.HasIndex(x => x.PublishedAt);
        b.HasIndex(x => x.Category);
    }
}
