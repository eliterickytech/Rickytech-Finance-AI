using FinanceControl.Data.Context;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Data.Seed;

/// <summary>
/// Popula a base com categorias e bancos default ao subir a aplicação pela primeira vez.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(FinanceControlDbContext db, CancellationToken ct = default)
    {
        await SeedCategoriesAsync(db, ct);
        await SeedBanksAsync(db, ct);
    }

    private static async Task SeedCategoriesAsync(FinanceControlDbContext db, CancellationToken ct)
    {
        if (await db.Categories.AnyAsync(ct)) return;

        var defaults = new (string Name, CategoryType Type, string Color, string Icon)[]
        {
            // Receitas
            ("Salário",                  CategoryType.Income,  "#22C55E", "fa-money-bill"),
            ("Freelance",                CategoryType.Income,  "#10B981", "fa-laptop-code"),
            ("Rendimentos Investimento", CategoryType.Income,  "#059669", "fa-chart-line"),
            ("Reembolsos",               CategoryType.Income,  "#84CC16", "fa-rotate-left"),
            ("Depósito Cripto",          CategoryType.Income,  "#F59E0B", "fa-bitcoin"),
            // Despesas
            ("Moradia",                  CategoryType.Expense, "#EF4444", "fa-house"),
            ("Alimentação",              CategoryType.Expense, "#F97316", "fa-utensils"),
            ("Transporte",               CategoryType.Expense, "#EAB308", "fa-car"),
            ("Saúde",                    CategoryType.Expense, "#06B6D4", "fa-heart-pulse"),
            ("Educação",                 CategoryType.Expense, "#3B82F6", "fa-graduation-cap"),
            ("Lazer",                    CategoryType.Expense, "#8B5CF6", "fa-gamepad"),
            ("Assinaturas",              CategoryType.Expense, "#EC4899", "fa-repeat"),
            ("Impostos",                 CategoryType.Expense, "#DC2626", "fa-landmark"),
            ("Saque Cripto",             CategoryType.Expense, "#F59E0B", "fa-bitcoin"),
            // Ambos
            ("Transferência entre contas", CategoryType.Both,  "#6366F1", "fa-arrows-rotate"),
            ("Ajustes",                  CategoryType.Both,   "#6B7280", "fa-sliders"),
            ("Sem categoria",            CategoryType.Both,   "#6C757D", "fa-question")
        };

        foreach (var (name, type, color, icon) in defaults)
            db.Categories.Add(Category.Create(name, type, HexColor.Create(color), icon));

        await db.SaveChangesAsync(ct);
    }

    private static async Task SeedBanksAsync(FinanceControlDbContext db, CancellationToken ct)
    {
        if (await db.Banks.AnyAsync(ct)) return;

        var defaults = new (string Name, string Nickname, BankAccountType Type, string Currency)[]
        {
            ("Itaú",      "Itaú CC",       BankAccountType.ContaCorrente, "BRL"),
            ("Bradesco",  "Bradesco CC",   BankAccountType.ContaCorrente, "BRL"),
            ("Nubank",    "Nubank",        BankAccountType.ContaCorrente, "BRL"),
            ("Inter",     "Inter",         BankAccountType.ContaCorrente, "BRL"),
            ("BTG",       "BTG Pactual",   BankAccountType.Corretora,     "BRL"),
            ("XP",        "XP Investimentos", BankAccountType.Corretora,  "BRL"),
            ("Binance",   "Binance Spot",  BankAccountType.Cripto,        "USDT"),
            ("Coinbase",  "Coinbase",      BankAccountType.Cripto,        "USD"),
            ("MetaMask",  "MetaMask",      BankAccountType.Carteira,      "ETH"),
            ("Em Espécie","Carteira",      BankAccountType.Carteira,      "BRL")
        };

        foreach (var (name, nickname, type, currency) in defaults)
            db.Banks.Add(Bank.Create(name, nickname, type, currency, 0m));

        await db.SaveChangesAsync(ct);
    }
}
