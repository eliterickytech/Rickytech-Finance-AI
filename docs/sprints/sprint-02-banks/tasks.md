# Sprint 2 - Tasks

> Estimativa: **21 pts**

## US-S2-01 - Entidade Bank (5 pts)
- [ ] `Bank : BaseEntity` (Nome, Apelido, Tipo, Agencia, Conta, Moeda, OpeningBalance, Active)
- [ ] ValueObject `Money(decimal Amount, string Currency)`
- [ ] Enum `BankAccountType`
- [ ] `BankConfiguration` (precision decimal(18,8) para suportar cripto)

## US-S2-02 - Migration (2 pts)
- [ ] `20260615_0000_Sprint02_Banks`

## US-S2-03 - Commands + Queries (5 pts)
- [ ] CreateBank / UpdateBank / DeleteBank
- [ ] GetBankById / GetAllBanks (filtros por tipo e moeda)

## US-S2-04 - Validators (2 pts)
- [ ] Moeda em lista permitida: BRL, USD, EUR, USDT, BTC, ETH, ADA, SOL
- [ ] OpeningBalance >= 0
- [ ] Tipo cripto exige moeda cripto

## US-S2-05 - Controller (2 pts)
- [ ] `BancosController : BaseApiController`

## US-S2-06 - Seed (2 pts)
- [ ] Itaú, Bradesco, Nubank, Inter, BTG, XP, Binance, Coinbase, MetaMask, Carteira em Espécie

## US-S2-07 - Testes (3 pts)
- [ ] Unit + Integration + API tests
