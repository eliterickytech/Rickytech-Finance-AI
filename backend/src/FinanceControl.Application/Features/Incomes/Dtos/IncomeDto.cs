using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Features.Incomes.Dtos;

public sealed record IncomeDto(
    Guid Id, string Description, decimal Amount, DateOnly Date,
    Guid CategoryId, Guid BankId, string[] Tags,
    RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd,
    string? IntegrationSourceId, string? AttachmentPath, bool Confirmed);

public sealed record CreateIncomeDto(
    string Description, decimal Amount, DateOnly Date,
    Guid CategoryId, Guid BankId, string[]? Tags,
    RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd);

public sealed record UpdateIncomeDto(
    string Description, decimal Amount, DateOnly Date,
    Guid CategoryId, Guid BankId, string[]? Tags,
    RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd);
