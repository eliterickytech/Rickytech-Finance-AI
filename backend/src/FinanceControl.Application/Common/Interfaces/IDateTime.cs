namespace FinanceControl.Application.Common.Interfaces;

/// <summary>
/// Abstração sobre DateTime/DateTimeOffset.UtcNow para permitir mock em testes.
/// </summary>
public interface IDateTime
{
    DateTimeOffset UtcNow { get; }
    DateOnly Today { get; }
}
