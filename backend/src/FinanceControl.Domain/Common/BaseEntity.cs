namespace FinanceControl.Domain.Common;

/// <summary>
/// Entidade base para todas as entidades do domínio - rastreia Id e timestamps.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }

    public void MarkAsUpdated() => UpdatedAt = DateTimeOffset.UtcNow;
    public void SoftDelete() { IsDeleted = true; MarkAsUpdated(); }
}
