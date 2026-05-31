using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Features.Expenses.Dtos;

public sealed record ExpenseDto(
    Guid Id, string Description, decimal Amount, DateOnly Date,
    Guid CategoryId, Guid BankId, string[] Tags, PaymentMethod PaymentMethod,
    RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd,
    string? IntegrationSourceId, string? AttachmentPath, bool Confirmed);

public sealed record CreateExpenseDto(
    string Description, decimal Amount, DateOnly Date,
    Guid CategoryId, Guid BankId, PaymentMethod PaymentMethod,
    string[]? Tags, RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd);

public sealed record UpdateExpenseDto(
    string Description, decimal Amount, DateOnly Date,
    Guid CategoryId, Guid BankId, PaymentMethod PaymentMethod,
    string[]? Tags, RecurrenceFrequency Recurrence, DateOnly? RecurrenceEnd);
