using System.ServiceModel.Syndication;
using System.Xml;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.CrossCutting.Configurations;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinanceControl.Infrastructure.Integrations.News;

/// <summary>
/// Agregador de notícias via feeds RSS/Atom. Auto-tagging por dicionário em appsettings.
/// </summary>
public sealed class RssNewsAggregator : INewsAggregator
{
    private readonly HttpClient _http;
    private readonly NewsOptions _options;
    private readonly ILogger<RssNewsAggregator> _logger;

    public RssNewsAggregator(HttpClient http, IOptions<IntegrationsOptions> options, ILogger<RssNewsAggregator> logger)
    {
        _http = http;
        _options = options.Value.News;
        _logger = logger;
    }

    public async Task<IReadOnlyList<NewsItem>> FetchAllAsync(CancellationToken ct)
    {
        var sources = _options.Sources ?? new List<NewsSource>();
        if (sources.Count == 0) sources = DefaultSources();

        var results = new List<NewsItem>();

        foreach (var source in sources)
        {
            try
            {
                var category = ParseCategory(source.Category);
                using var stream = await _http.GetStreamAsync(source.Url, ct);
                using var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true, DtdProcessing = DtdProcessing.Ignore });
                var feed = SyndicationFeed.Load(reader);
                if (feed is null) continue;

                foreach (var item in feed.Items.Take(50))
                {
                    var url = item.Links.FirstOrDefault()?.Uri?.ToString();
                    if (string.IsNullOrWhiteSpace(url)) continue;

                    var tags = ExtractTags(item.Title?.Text ?? string.Empty, item.Summary?.Text ?? string.Empty);
                    results.Add(NewsItem.Create(
                        title: item.Title?.Text ?? "(sem título)",
                        url: url,
                        source: source.Name,
                        category: category,
                        publishedAt: item.PublishDate == default ? DateTimeOffset.UtcNow : item.PublishDate,
                        tags: tags,
                        summary: item.Summary?.Text,
                        imageUrl: null));
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao carregar feed {Source} ({Url})", source.Name, source.Url);
            }
        }

        return results;
    }

    private string[] ExtractTags(string title, string summary)
    {
        var text = $"{title} {summary}".ToLowerInvariant();
        var tags = new HashSet<string>();
        var dict = _options.TagDictionary ?? DefaultTagDictionary();
        foreach (var (tag, keywords) in dict)
        {
            if (keywords.Any(k => text.Contains(k.ToLowerInvariant())))
                tags.Add(tag);
        }
        return tags.ToArray();
    }

    private static NewsCategory ParseCategory(string c) => c?.ToLowerInvariant() switch
    {
        "crypto"        => NewsCategory.Crypto,
        "financialbr"   => NewsCategory.FinancialBR,
        "financialintl" => NewsCategory.FinancialIntl,
        _               => NewsCategory.FinancialBR
    };

    private static List<NewsSource> DefaultSources() => new()
    {
        new() { Name = "CoinDesk",      Url = "https://www.coindesk.com/arc/outboundfeeds/rss/", Category = "Crypto" },
        new() { Name = "CoinTelegraph", Url = "https://cointelegraph.com/rss",                    Category = "Crypto" },
        new() { Name = "InfoMoney",     Url = "https://www.infomoney.com.br/feed/",               Category = "FinancialBR" }
    };

    private static Dictionary<string, string[]> DefaultTagDictionary() => new()
    {
        ["BTC"]   = new[] { "bitcoin", "btc" },
        ["ETH"]   = new[] { "ethereum", "eth", "ether" },
        ["ADA"]   = new[] { "cardano", "ada" },
        ["SOL"]   = new[] { "solana", "sol" },
        ["BNB"]   = new[] { "binance coin", "bnb" },
        ["IBOV"]  = new[] { "ibovespa", "ibov" },
        ["Selic"] = new[] { "selic", "copom" },
        ["FED"]   = new[] { "federal reserve", "powell" }
    };
}
