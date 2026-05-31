using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Domain.Common;
using FinanceControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Data.Context;

/// <summary>
/// DbContext principal do Finance Control. Implementa IApplicationDbContext
/// para que a camada Application acesse o banco apenas via abstração.
/// </summary>
public sealed class FinanceControlDbContext : DbContext, IApplicationDbContext
{
    public FinanceControlDbContext(DbContextOptions<FinanceControlDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Investment> Investments => Set<Investment>();
    public DbSet<InvestmentOperation> InvestmentOperations => Set<InvestmentOperation>();
    public DbSet<AssetQuote> AssetQuotes => Set<AssetQuote>();
    public DbSet<IntegrationConfig> IntegrationConfigs => Set<IntegrationConfig>();
    public DbSet<NewsItem> NewsItems => Set<NewsItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceControlDbContext).Assembly);

        // Soft delete global: aplica HasQueryFilter em todas as BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(BuildSoftDeleteFilter(entityType.ClrType));
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    private static System.Linq.Expressions.LambdaExpression BuildSoftDeleteFilter(Type type)
    {
        var parameter = System.Linq.Expressions.Expression.Parameter(type, "e");
        var prop = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
        var notDeleted = System.Linq.Expressions.Expression.Equal(prop, System.Linq.Expressions.Expression.Constant(false));
        return System.Linq.Expressions.Expression.Lambda(notDeleted, parameter);
    }
}
