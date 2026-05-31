using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Common.Interfaces;

/// <summary>
/// Abstração do DbContext exposta para a camada Application.
/// As entidades vão sendo adicionadas conforme cada Sprint.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Bank> Banks { get; }
    DbSet<Income> Incomes { get; }
    DbSet<Expense> Expenses { get; }
    DbSet<Investment> Investments { get; }
    DbSet<InvestmentOperation> InvestmentOperations { get; }
    DbSet<AssetQuote> AssetQuotes { get; }
    DbSet<IntegrationConfig> IntegrationConfigs { get; }
    DbSet<NewsItem> NewsItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
