using FinanceControl.Domain.Common;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

public sealed class Bank : BaseEntity
{
    private static readonly string[] AllowedCurrencies =
        ["BRL", "USD", "EUR", "USDT", "USDC", "BTC", "ETH", "ADA", "SOL", "BNB", "MATIC", "DOT", "AVAX", "DOGE"];

    public string Name { get; private set; } = string.Empty;
    public string Nickname { get; private set; } = string.Empty;
    public BankAccountType Type { get; private set; }
    public string? Branch { get; private set; }
    public string? AccountNumber { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public decimal OpeningBalance { get; private set; }
    public bool Active { get; private set; } = true;

    private Bank() { }

    public static Bank Create(
        string name, string nickname, BankAccountType type, string currency,
        decimal openingBalance, string? branch = null, string? accountNumber = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Nome do banco é obrigatório.");
        if (string.IsNullOrWhiteSpace(nickname)) throw new DomainException("Apelido é obrigatório.");
        if (openingBalance < 0) throw new DomainException("Saldo inicial não pode ser negativo.");

        var cur = currency.ToUpperInvariant();
        if (!AllowedCurrencies.Contains(cur))
            throw new DomainException($"Moeda '{currency}' não está habilitada.");

        if (type == BankAccountType.Cripto && cur is "BRL" or "USD" or "EUR")
            throw new DomainException("Conta Cripto exige moeda cripto (BTC/ETH/USDT/...).");

        return new Bank
        {
            Name = name.Trim(),
            Nickname = nickname.Trim(),
            Type = type,
            Currency = cur,
            OpeningBalance = openingBalance,
            Branch = branch?.Trim(),
            AccountNumber = accountNumber?.Trim()
        };
    }

    public void Rename(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname)) throw new DomainException("Apelido é obrigatório.");
        Nickname = nickname.Trim();
        MarkAsUpdated();
    }

    public void Activate() { Active = true; MarkAsUpdated(); }
    public void Deactivate() { Active = false; MarkAsUpdated(); }
}
