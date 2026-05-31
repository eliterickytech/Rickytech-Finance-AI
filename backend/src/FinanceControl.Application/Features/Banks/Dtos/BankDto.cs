using FinanceControl.Domain.Enums;

namespace FinanceControl.Application.Features.Banks.Dtos;

public sealed record BankDto(
    Guid Id, string Name, string Nickname, BankAccountType Type,
    string Currency, decimal OpeningBalance, decimal CurrentBalance,
    string? Branch, string? AccountNumber, bool Active);

public sealed record CreateBankDto(
    string Name, string Nickname, BankAccountType Type, string Currency,
    decimal OpeningBalance, string? Branch, string? AccountNumber);

public sealed record UpdateBankDto(string Nickname, bool Active);
