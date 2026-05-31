namespace FinanceControl.Domain.Exceptions;

/// <summary>
/// Exception base do Domain para violações de invariantes.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
