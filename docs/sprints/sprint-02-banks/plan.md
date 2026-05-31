# Sprint 2 - CRUD de Bancos

## Objetivo
Entregar o CRUD de **Bancos / Contas** (incluindo carteiras cripto), pilar
para receitas, despesas e investimentos a partir do Sprint 3.

## Escopo IN
- Entidade `Bank` em Domain.
- Enum `BankAccountType { ContaCorrente, Poupanca, Carteira, Corretora, Cripto }`.
- ValueObject `Money` (amount + currency ISO 4217 ou símbolo cripto).
- CRUD MediatR + Controller + Validators + Profile.
- Migration `20260615_0000_Sprint02_Banks`.
- Cálculo de saldo atual: persistido + ajustável manualmente (campo `OpeningBalance` + soma de movimentos a partir do Sprint 3 quando integrado).
- Seed com bancos populares: Itaú, Bradesco, Nubank, Inter, BTG, XP, Binance, Coinbase, MetaMask.

## Escopo OUT
- Integração Open Finance (Sprint 7).
- Integração Binance / cripto (Sprint 6).

## Decisões
- `MoedaPadrao` é `string(10)` para acomodar tickers cripto (`USDT`, `BTC`, `ETH`, `ADA`, `SOL`).
- Saldo inicial e moeda são **imutáveis** após criação (alterar exige criar conta nova) — proteção contra reescrita histórica.

## Critério de pronto (DoD)
- `POST /api/v1/bancos` cria conta com saldo inicial e moeda.
- Listagem retorna saldo atual calculado (no Sprint 2 = OpeningBalance, pois sem lançamentos).
- Validação de moeda contra lista permitida.
- Testes >= 80%.
