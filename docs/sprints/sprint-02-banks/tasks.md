# Sprint 2 - Tasks

> Estimativa: **21 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S2-01 - Entidade Bank (5 pts)
- [x] `Bank : BaseEntity` (Nome, Apelido, Tipo, Agencia, Conta, Moeda, OpeningBalance, Active)
- [x] ValueObject `Money(decimal Amount, string Currency)`
- [x] Enum `BankAccountType`
- [x] `BankConfiguration` (precision decimal(18,8) para suportar cripto)

## US-S2-02 - Migration (2 pts)
- [ ] `20260615_0000_Sprint02_Banks` — **NÃO gerada**

## US-S2-03 - Commands + Queries (5 pts)
- [x] CreateBank / UpdateBank / DeleteBank
- [x] GetBankById / GetAllBanks (filtros por tipo e moeda)

## US-S2-04 - Validators (2 pts)
- [x] Moeda em lista permitida: BRL, USD, EUR, USDT, BTC, ETH, ADA, SOL
- [x] OpeningBalance >= 0
- [x] Tipo cripto exige moeda cripto

## US-S2-05 - Controller (2 pts)
- [x] `BancosController : BaseApiController`

## US-S2-06 - Seed (2 pts)
- [x] Itaú, Bradesco, Nubank, Inter, BTG, XP, Binance, Coinbase, MetaMask, Carteira em Espécie

## US-S2-07 - Testes (3 pts)
- [ ] Unit + Integration + API tests — **só existe teste de entidade `BankTests`** (unit de domínio)
