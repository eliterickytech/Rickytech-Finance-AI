using System.Security.Cryptography;
using System.Text;
using FinanceControl.Domain.Common;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

public sealed class NewsItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public string UrlHash { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;
    public NewsCategory Category { get; private set; }
    public string[] Tags { get; private set; } = Array.Empty<string>();
    public DateTimeOffset PublishedAt { get; private set; }
    public string? Summary { get; private set; }
    public string? ImageUrl { get; private set; }

    private NewsItem() { }

    public static NewsItem Create(
        string title, string url, string source, NewsCategory category,
        DateTimeOffset publishedAt, string[]? tags = null, string? summary = null, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new DomainException("Título obrigatório.");
        if (string.IsNullOrWhiteSpace(url)) throw new DomainException("URL obrigatória.");
        if (string.IsNullOrWhiteSpace(source)) throw new DomainException("Source obrigatório.");

        return new NewsItem
        {
            Title = title.Trim(),
            Url = url.Trim(),
            UrlHash = ComputeUrlHash(url),
            Source = source.Trim(),
            Category = category,
            Tags = tags ?? Array.Empty<string>(),
            PublishedAt = publishedAt,
            Summary = summary?.Trim(),
            ImageUrl = imageUrl?.Trim()
        };
    }

    public static string ComputeUrlHash(string url)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(url.Trim().ToLowerInvariant()));
        return Convert.ToHexString(bytes);
    }
}
