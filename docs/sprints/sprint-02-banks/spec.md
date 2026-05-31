# Sprint 2 - Spec técnica (Bancos)

## Entidade

```csharp
public sealed class Bank : BaseEntity
{
    public string Name { get; private set; }
    public string Nickname { get; private set; }
    public BankAccountType Type { get; private set; }
    public string? Branch { get; private set; }
    public string? AccountNumber { get; private set; }
    public string Currency { get; private set; }              // BRL, USD, USDT, BTC, ETH, ...
    public decimal OpeningBalance { get; private set; }
    public bool Active { get; private set; } = true;

    public static Bank Create(string name, string nickname, BankAccountType type,
        string currency, decimal openingBalance, string? branch = null, string? accountNumber = null);

    public void Rename(string nickname);
    public void Disable();
}
```

## DTOs

```csharp
public record BankDto(Guid Id, string Name, string Nickname, BankAccountType Type, string Currency,
    decimal OpeningBalance, decimal CurrentBalance, bool Active);

public record CreateBankDto(string Name, string Nickname, BankAccountType Type, string Currency,
    decimal OpeningBalance, string? Branch, string? AccountNumber);
```

## Endpoints

| Verbo  | Rota                       | Status   |
|--------|----------------------------|----------|
| POST   | `/api/v1/bancos`           | 201      |
| GET    | `/api/v1/bancos/{id}`      | 200      |
| GET    | `/api/v1/bancos`           | 200      |
| PUT    | `/api/v1/bancos/{id}`      | 200      |
| DELETE | `/api/v1/bancos/{id}`      | 204      |

## Validators

- `Currency` em `{ BRL, USD, EUR, USDT, USDC, BTC, ETH, ADA, SOL, BNB }`
- `Type == Cripto` exige `Currency` cripto
- `Type == Corretora` aceita BRL ou USD
- `OpeningBalance >= 0`
- `Nickname` único por usuário

## Seed

```
- Itaú        (ContaCorrente, BRL)
- Bradesco    (ContaCorrente, BRL)
- Nubank      (ContaCorrente, BRL)
- Inter       (ContaCorrente, BRL)
- BTG         (Corretora, BRL)
- XP          (Corretora, BRL)
- Binance     (Cripto, USDT)
- Coinbase    (Cripto, USD)
- MetaMask    (Carteira, ETH)
- Em Espécie  (Carteira, BRL)
```
