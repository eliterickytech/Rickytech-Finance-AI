namespace FinanceControl.Application.Common.Models;

/// <summary>
/// Envelope padrão para todas as respostas da API.
/// </summary>
public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message,
    IReadOnlyCollection<string>? Errors,
    DateTimeOffset Timestamp)
{
    public static ApiResponse<T> SuccessResult(T? data, string? message = null) =>
        new(true, data, message, null, DateTimeOffset.UtcNow);

    public static ApiResponse<T> FailureResult(string message, IEnumerable<string>? errors = null) =>
        new(false, default, message, errors?.ToArray(), DateTimeOffset.UtcNow);
}
