using FinanceControl.Application.Common.Interfaces;
using FinanceControl.CrossCutting.Configurations;
using FinanceControl.Data.Context;
using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinanceControl.Infrastructure.Integrations.News;

/// <summary>
/// Background service que atualiza o cache de notícias periodicamente.
/// </summary>
public sealed class NewsRefreshHostedService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly NewsOptions _options;
    private readonly ILogger<NewsRefreshHostedService> _logger;

    public NewsRefreshHostedService(
        IServiceProvider services,
        IOptions<IntegrationsOptions> options,
        ILogger<NewsRefreshHostedService> logger)
    {
        _services = services;
        _options = options.Value.News;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Atraso inicial para garantir que migrations rodaram
        await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);

        var interval = TimeSpan.FromMinutes(Math.Max(1, _options.RefreshIntervalMinutes));

        while (!stoppingToken.IsCancellationRequested)
        {
            try { await RefreshOnceAsync(stoppingToken); }
            catch (Exception ex) { _logger.LogError(ex, "Falha no refresh de notícias."); }

            await Task.Delay(interval, stoppingToken);
        }
    }

    private async Task RefreshOnceAsync(CancellationToken ct)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FinanceControlDbContext>();
        var aggregator = scope.ServiceProvider.GetRequiredService<INewsAggregator>();

        var items = await aggregator.FetchAllAsync(ct);
        _logger.LogInformation("Coletadas {Count} notícias dos feeds.", items.Count);

        var inserted = 0;
        foreach (var item in items)
        {
            var exists = await db.NewsItems.AnyAsync(n => n.UrlHash == item.UrlHash, ct);
            if (exists) continue;
            db.NewsItems.Add(item);
            inserted++;
        }

        // TTL: remove notícias antigas
        var cutoff = DateTimeOffset.UtcNow.AddDays(-_options.RetentionDays);
        var old = await db.NewsItems.Where(n => n.PublishedAt < cutoff).ToListAsync(ct);
        foreach (var n in old) n.SoftDelete();

        await db.SaveChangesAsync(ct);
        _logger.LogInformation("News refresh: {Inserted} inseridas, {Removed} expiradas.", inserted, old.Count);
    }
}
