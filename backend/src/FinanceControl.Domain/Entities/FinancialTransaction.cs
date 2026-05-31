using FinanceControl.Domain.Common;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

/// <summary>
/// Classe base abstrata para Income/Expense (TPH no EF Core).
/// </summary>
public abstract class FinancialTransaction : BaseEntity
{
    public string Description { get; protected set; } = string.Empty;
    public decimal Amount { get; protected set; }
    public DateOnly Date { get; protected set; }
    public Guid CategoryId { get; protected set; }
    public Category? Category { get; protected set; }
    public Guid BankId { get; protected set; }
    public Bank? Bank { get; protected set; }
    public string[] Tags { get; protected set; } = Array.Empty<string>();
    public RecurrenceFrequency Recurrence { get; protected set; } = RecurrenceFrequency.Once;
    public DateOnly? RecurrenceEnd { get; protected set; }
    public string? IntegrationSourceId { get; protected set; }
    public string? AttachmentPath { get; protected set; }
    public bool Confirmed { get; protected set; } = true;

    protected void Init(string description, decimal amount, DateOnly date, Guid categoryId, Guid bankId,
        string[]? tags, RecurrenceFrequency recurrence, DateOnly? recurrenceEnd,
        string? integrationSourceId, bool confirmed)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new DomainException("Descrição é obrigatória.");
        if (amount <= 0) throw new DomainException("Valor deve ser positivo.");
        if (categoryId == Guid.Empty) throw new DomainException("Categoria obrigatória.");
        if (bankId == Guid.Empty) throw new DomainException("Banco obrigatório.");
        if (recurrence != RecurrenceFrequency.Once && recurrenceEnd is { } end && end < date)
            throw new DomainException("RecurrenceEnd deve ser maior ou igual à data inicial.");

        Description = description.Trim();
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
        BankId = bankId;
        Tags = tags ?? Array.Empty<string>();
        Recurrence = recurrence;
        RecurrenceEnd = recurrenceEnd;
        IntegrationSourceId = integrationSourceId;
        Confirmed = confirmed;
    }

    public void Update(string description, decimal amount, DateOnly date, Guid categoryId, Guid bankId,
        string[]? tags, RecurrenceFrequency recurrence, DateOnly? recurrenceEnd)
    {
        Init(description, amount, date, categoryId, bankId, tags, recurrence, recurrenceEnd, IntegrationSourceId, Confirmed);
        MarkAsUpdated();
    }

    public void Confirm() { Confirmed = true; MarkAsUpdated(); }
    public void Unconfirm() { Confirmed = false; MarkAsUpdated(); }
    public void SetAttachment(string path) { AttachmentPath = path; MarkAsUpdated(); }
}
