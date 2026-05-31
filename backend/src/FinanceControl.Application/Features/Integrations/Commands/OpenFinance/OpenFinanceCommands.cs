using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Integrations.Dtos;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FinanceControl.Application.Features.Integrations.Commands.OpenFinance;

public sealed record StartConsentCommand(string Cpf, string BankCode) : IRequest<ConsentUrlDto>;

public sealed class StartConsentValidator : AbstractValidator<StartConsentCommand>
{
    public StartConsentValidator()
    {
        RuleFor(x => x.Cpf).NotEmpty().Length(11);
        RuleFor(x => x.BankCode).NotEmpty().MaximumLength(20);
    }
}

public sealed class StartConsentHandler : IRequestHandler<StartConsentCommand, ConsentUrlDto>
{
    private readonly IOpenFinanceClient _client;
    private readonly IApplicationDbContext _db;
    private readonly ICryptoCipher _cipher;
    public StartConsentHandler(IOpenFinanceClient client, IApplicationDbContext db, ICryptoCipher cipher)
    { _client = client; _db = db; _cipher = cipher; }

    public async Task<ConsentUrlDto> Handle(StartConsentCommand r, CancellationToken ct)
    {
        var consent = await _client.CreateConsentAsync(r.Cpf, r.BankCode, ct);

        var existing = await _db.IntegrationConfigs.FirstOrDefaultAsync(c => c.Provider == IntegrationProvider.OpenFinance, ct);
        var meta = JsonSerializer.Serialize(new Dictionary<string, string>
        {
            ["consentId"] = consent.ConsentId,
            ["bankCode"] = r.BankCode
        });

        if (existing is null)
        {
            existing = IntegrationConfig.Create(IntegrationProvider.OpenFinance,
                _cipher.Encrypt("pending"), null, meta);
            _db.IntegrationConfigs.Add(existing);
        }
        else
        {
            existing.UpdateMetadata(meta);
        }
        await _db.SaveChangesAsync(ct);

        return new ConsentUrlDto(consent.ConsentUrl, consent.ConsentId);
    }
}

public sealed record HandleCallbackCommand(string Code, string State) : IRequest;

public sealed class HandleCallbackHandler : IRequestHandler<HandleCallbackCommand>
{
    private readonly IOpenFinanceClient _client;
    private readonly IApplicationDbContext _db;
    private readonly ICryptoCipher _cipher;

    public HandleCallbackHandler(IOpenFinanceClient client, IApplicationDbContext db, ICryptoCipher cipher)
    { _client = client; _db = db; _cipher = cipher; }

    public async Task Handle(HandleCallbackCommand r, CancellationToken ct)
    {
        var token = await _client.ExchangeCodeAsync(r.Code, r.State, ct);
        var cfg = await _db.IntegrationConfigs.FirstOrDefaultAsync(c => c.Provider == IntegrationProvider.OpenFinance, ct)
            ?? throw new NotFoundException("IntegrationConfig.OpenFinance", "n/a");

        cfg.UpdateCredentials(_cipher.Encrypt(token.AccessToken), _cipher.Encrypt(token.RefreshToken));
        cfg.UpdateMetadata(JsonSerializer.Serialize(new Dictionary<string, string>
        {
            ["expiresAt"] = token.ExpiresAt.ToString("O")
        }));
        cfg.Activate();
        await _db.SaveChangesAsync(ct);
    }
}

public sealed record SyncOpenFinanceCommand : IRequest<SyncResultDto>;

public sealed class SyncOpenFinanceHandler : IRequestHandler<SyncOpenFinanceCommand, SyncResultDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IOpenFinanceClient _client;
    private readonly ICryptoCipher _cipher;
    private readonly ITransactionClassifier _classifier;
    private readonly IDateTime _dateTime;

    public SyncOpenFinanceHandler(IApplicationDbContext db, IOpenFinanceClient client,
        ICryptoCipher cipher, ITransactionClassifier classifier, IDateTime dateTime)
    {
        _db = db; _client = client; _cipher = cipher; _classifier = classifier; _dateTime = dateTime;
    }

    public async Task<SyncResultDto> Handle(SyncOpenFinanceCommand r, CancellationToken ct)
    {
        var cfg = await _db.IntegrationConfigs.FirstOrDefaultAsync(c => c.Provider == IntegrationProvider.OpenFinance, ct)
            ?? throw new NotFoundException("IntegrationConfig.OpenFinance", "n/a");

        if (!cfg.Active) throw new UnauthorizedException("Open Finance não conectado.");

        var accessToken = _cipher.Decrypt(cfg.ApiKeyEncrypted);

        var categories = await _db.Categories.AsNoTracking().ToListAsync(ct);
        var catIndex = categories.GroupBy(c => c.Name).ToDictionary(g => g.Key, g => g.First().Id);

        var since = cfg.LastSyncAt is { } last
            ? DateOnly.FromDateTime(last.UtcDateTime)
            : _dateTime.Today.AddMonths(-3);

        var imported = 0; var skipped = 0;

        await foreach (var account in _client.GetAccountsAsync(accessToken, ct))
        {
            var bank = await EnsureBankAsync(account, ct);

            await foreach (var tx in _client.GetTransactionsAsync(accessToken, account.AccountId, since, ct))
            {
                var sourceId = $"OFI:{tx.TransactionId}";
                var isCredit = tx.CreditDebitType.Equals("CREDIT", StringComparison.OrdinalIgnoreCase);

                if (isCredit && await _db.Incomes.AnyAsync(i => i.IntegrationSourceId == sourceId, ct))
                { skipped++; continue; }
                if (!isCredit && await _db.Expenses.AnyAsync(e => e.IntegrationSourceId == sourceId, ct))
                { skipped++; continue; }

                var categoryId = _classifier.Classify(tx.Description, isCredit, catIndex);
                if (categoryId == Guid.Empty) { skipped++; continue; }

                if (isCredit)
                {
                    var income = Income.Create(tx.Description, tx.Amount, tx.Date, categoryId, bank.Id,
                        integrationSourceId: sourceId);
                    _db.Incomes.Add(income);
                }
                else
                {
                    var expense = Expense.Create(tx.Description, tx.Amount, tx.Date, categoryId, bank.Id,
                        PaymentMethod.TransferBank, integrationSourceId: sourceId);
                    _db.Expenses.Add(expense);
                }
                imported++;
            }
        }

        cfg.TouchSync(_dateTime.UtcNow);
        await _db.SaveChangesAsync(ct);

        return new SyncResultDto(imported, skipped, _dateTime.UtcNow);
    }

    private async Task<Bank> EnsureBankAsync(OfiAccount acc, CancellationToken ct)
    {
        var nickname = $"OFI-{acc.AccountId}";
        var existing = await _db.Banks.FirstOrDefaultAsync(b => b.Nickname == nickname, ct);
        if (existing is not null) return existing;
        var bank = Bank.Create("Open Finance", nickname, BankAccountType.ContaCorrente, acc.Currency, 0m);
        _db.Banks.Add(bank);
        await _db.SaveChangesAsync(ct);
        return bank;
    }
}
