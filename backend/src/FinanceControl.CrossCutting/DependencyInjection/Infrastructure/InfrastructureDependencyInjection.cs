using FinanceControl.Application.Common.Interfaces;
using FinanceControl.CrossCutting.Configurations;
using FinanceControl.Data.Context;
using FinanceControl.Data.Interceptors;
using FinanceControl.Infrastructure.Authentication;
using FinanceControl.Infrastructure.Integrations.Binance;
using FinanceControl.Infrastructure.Integrations.News;
using FinanceControl.Infrastructure.Integrations.OpenFinance;
using FinanceControl.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceControl.CrossCutting.DependencyInjection;

/// <summary>
/// Registra DbContext, Repositórios e clientes HTTP de integrações.
/// </summary>
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // --- Options (fortemente tipadas) ---
        services.Configure<IntegrationsOptions>(configuration.GetSection(IntegrationsOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

        // --- Date/Time abstraction + Crypto ---
        services.AddSingleton<IDateTime, DateTimeService>();
        services.AddSingleton<ICryptoCipher, AesCryptoCipher>();
        services.AddSingleton<IRecurrenceProjector, RecurrenceProjector>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        // --- Interceptors ---
        services.AddScoped<AuditInterceptor>();

        // --- DbContext (provider chaveado por appsettings) ---
        var dbProvider = configuration["Database:Provider"] ?? "SqlServer";

        services.AddDbContext<FinanceControlDbContext>((sp, options) =>
        {
            var audit = sp.GetRequiredService<AuditInterceptor>();

            switch (dbProvider)
            {
                case "Sqlite":
                    options.UseSqlite(configuration.GetConnectionString("FinanceControlDbSqlite"));
                    break;
                default:
                    options.UseSqlServer(
                        configuration.GetConnectionString("FinanceControlDb"),
                        sql => sql.MigrationsAssembly("FinanceControl.Data"));
                    break;
            }

            options.AddInterceptors(audit);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<FinanceControlDbContext>());

        // --- HTTP / Cache ---
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddHttpContextAccessor();

        // --- Binance ---
        services.AddHttpClient<IBinanceClient, BinanceClient>(c =>
        {
            c.BaseAddress = new Uri("https://api.binance.com");
            c.Timeout = TimeSpan.FromSeconds(15);
        });
        services.AddScoped<IQuoteProvider, BinanceQuoteProvider>();

        // --- Open Finance ---
        services.AddHttpClient<IOpenFinanceClient, OpenFinanceClient>(c =>
        {
            c.Timeout = TimeSpan.FromSeconds(30);
        });
        services.AddSingleton<ITransactionClassifier, KeywordTransactionClassifier>();

        // --- News ---
        services.AddHttpClient<INewsAggregator, RssNewsAggregator>(c =>
        {
            c.Timeout = TimeSpan.FromSeconds(20);
            c.DefaultRequestHeaders.UserAgent.ParseAdd("FinanceControl/1.0 (+rickytech)");
        });
        services.AddHostedService<NewsRefreshHostedService>();

        return services;
    }
}
