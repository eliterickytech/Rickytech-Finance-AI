using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Integrations.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceControl.Application.Features.Integrations.Commands.Binance;

public sealed record SyncBinanceCommand : IRequest<SyncResultDto>;

public sealed class SyncBinanceHandler : IRequestHandler<SyncBinanceCommand, SyncResultDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICryptoCipher _cipher;
    private readonly IBinanceClient _binance;
    private readonly IDateTime _dateTime;
    private readonly ILogger<SyncBinanceHandler> _logger;

    public SyncBinanceHandler(IApplicationDbContext db, ICryptoCipher cipher,
        IBinanceClient binance, IDateTime dateTime, ILogger<SyncBinanceHandler> logger)
    {
        _db = db; _cipher = cipher; _binance = binance; _dateTime = dateTime; _logger = logger;
    }

    public async Task<SyncResultDto> Handle(SyncBinanceCommand r, CancellationToken ct)
    {
        var cfg = await _db.IntegrationConfigs.FirstOrDefaultAsync(c => c.Provider == IntegrationProvider.Binance, ct)
            ?? throw new NotFoundException("IntegrationConfig.Binance", "n/a");

        if (!cfg.Active) throw new UnauthorizedException("Integração Binance inativa.");

        var apiKey = _cipher.Decrypt(cfg.ApiKeyEncrypted);
        var apiSecret = _cipher.Decrypt(cfg.ApiSecretEncrypted ?? string.Empty);
        var since = cfg.LastSyncAt ?? _dateTime.UtcNow.AddDays(-90);

        var binanceBank = await EnsureBinanceBankAsync(ct);

        var imported = 0; var skipped = 0;

        // 1. Saldos → Investments
        var balances = await _binance.GetBalancesAsync(apiKey, apiSecret, ct);
        foreach (var bal in balances.Where(b => b.Free + b.Locked > 0))
        {
            var ticker = bal.Asset.ToUpperInvariant();
            var existing = await _db.Investments.FirstOrDefaultAsync(
                i => i.Ticker == ticker && i.BankId == binanceBank.Id, ct);

            var total = bal.Free + bal.Locked;
            if (existing is null)
            {
                var inv = Investment.Create(ticker, InvestmentType.Cripto, total, 0m, "USDT",
                    _dateTime.Today, binanceBank.Id);
                _db.Investments.Add(inv);
                imported++;
            }
            else { skipped++; }
        }

        // 2. Trades → InvestmentOperation
        var symbols = new[] { "BTCUSDT", "ETHUSDT", "ADAUSDT", "SOLUSDT", "BNBUSDT" };
        foreach (var sym in symbols)
        {
            var trades = await _binance.GetTradesAsync(apiKey, apiSecret, sym, since, ct);
            foreach (var t in trades)
            {
                var sourceId = $"BINANCE:TRADE:{t.Id}";
                if (await _db.InvestmentOperations.AnyAsync(o => o.IntegrationSourceId == sourceId, ct))
                { skipped++; continue; }

                var ticker = sym.Replace("USDT", "");
                var inv = await _db.Investments.FirstOrDefaultAsync(
                    i => i.Ticker == ticker && i.BankId == binanceBank.Id, ct);
                if (inv is null) continue;

                var op = InvestmentOperation.Create(inv.Id,
                    t.IsBuyer ? OperationSide.Buy : OperationSide.Sell,
                    t.Quantity, t.Price, t.Commission, t.Time, sourceId);
                inv.ApplyOperation(op);
                imported++;
            }
        }

        // 3. Depósitos → Income; Saques → Expense (catch-all categoria padrão)
        var defaultIncomeCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Name == "Depósito Cripto", ct);
        var defaultExpenseCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Name == "Saque Cripto", ct);

        if (defaultIncomeCategory is not null)
        {
            foreach (var dep in await _binance.GetDepositsAsync(apiKey, apiSecret, since, ct))
            {
                var sourceId = $"BINANCE:DEPOSIT:{dep.Id}";
                if (await _db.Incomes.AnyAsync(i => i.IntegrationSourceId == sourceId, ct))
                { skipped++; continue; }
                var income = Income.Create($"Depósito {dep.Asset}", dep.Amount,
                    DateOnly.FromDateTime(dep.Time.UtcDateTime),
                    defaultIncomeCategory.Id, binanceBank.Id, integrationSourceId: sourceId);
                _db.Incomes.Add(income);
                imported++;
            }
        }

        if (defaultExpenseCategory is not null)
        {
            foreach (var wd in await _binance.GetWithdrawalsAsync(apiKey, apiSecret, since, ct))
            {
                var sourceId = $"BINANCE:WITHDRAW:{wd.Id}";
                if (await _db.Expenses.AnyAsync(e => e.IntegrationSourceId == sourceId, ct))
                { skipped++; continue; }
                var expense = Expense.Create($"Saque {wd.Asset}", wd.Amount,
                    DateOnly.FromDateTime(wd.Time.UtcDateTime),
                    defaultExpenseCategory.Id, binanceBank.Id, PaymentMethod.Crypto,
                    integrationSourceId: sourceId);
                _db.Expenses.Add(expense);
                imported++;
            }
        }

        cfg.TouchSync(_dateTime.UtcNow);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Binance sync concluído: imported={Imported} skipped={Skipped}", imported, skipped);
        return new SyncResultDto(imported, skipped, _dateTime.UtcNow);
    }

    private async Task<Bank> EnsureBinanceBankAsync(CancellationToken ct)
    {
        var bank = await _db.Banks.FirstOrDefaultAsync(b => b.Name == "Binance" && b.Type == BankAccountType.Cripto, ct);
        if (bank is not null) return bank;
        bank = Bank.Create("Binance", "Binance Spot", BankAccountType.Cripto, "USDT", 0m);
        _db.Banks.Add(bank);
        await _db.SaveChangesAsync(ct);
        return bank;
    }
}
