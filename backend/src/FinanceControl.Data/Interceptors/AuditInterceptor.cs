using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FinanceControl.Data.Interceptors;

/// <summary>
/// SaveChanges interceptor que carimba CreatedAt / UpdatedAt em todas as BaseEntity.
/// </summary>
public sealed class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IDateTime _dateTime;

    public AuditInterceptor(IDateTime dateTime) => _dateTime = dateTime;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateTimestamps(DbContext? context)
    {
        if (context is null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue = _dateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = _dateTime.UtcNow;
                    break;
            }
        }
    }
}
