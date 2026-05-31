namespace FinanceControl.Application.Common.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyCollection<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Uma ou mais regras de validação falharam.")
    {
        Errors = errors.ToArray();
    }
}

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entity, object key)
        : base($"{entity} com chave '{key}' não foi encontrada.") { }
}

public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Não autorizado.") : base(message) { }
}
