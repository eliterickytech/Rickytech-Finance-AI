# Sprint 4 - CRUD de Investimentos

## Objetivo
Modelar e expor o CRUD de **Investimentos**, abrangendo Renda Fixa,
Renda Variável e **Cripto** (BTC, ETH, ADA Cardano, SOL Solana, BNB, ...).

## Escopo IN
- Entidade `Investment` com tipo, ativo (ticker), quantidade, preço médio.
- Enum `InvestmentType { RendaFixa, Acao, FII, ETF, Cripto, Outro }`.
- Entidade `AssetQuote` (cache de cotação) por (ticker, data).
- Service `IQuoteProvider` (abstração) com **mock** no Sprint 4; implementação real (Binance + CoinGecko) no Sprint 6.
- Cálculo automático: ValorAtual = Quantidade × CotaçãoMaisRecente; Lucro/Prejuízo = ValorAtual - (Quantidade × PrecoMedio).
- Histórico de aportes (operações de buy/sell) — entidade `InvestmentOperation`.
- CRUD MediatR + endpoints.
- Migration `20260713_0000_Sprint04_Investments`.

## Escopo OUT
- Importação automática de operações (Sprint 6 - Binance, Sprint 7 - corretoras).
- Cálculo de IR sobre cripto / day-trade (sprint futuro).

## Decisões
- Quantidade é `decimal(28,18)` para acomodar precisões cripto (ex.: BTC com 8 casas, satoshis).
- Preço médio é recalculado a cada operação (média ponderada).
- `AssetQuote` é populado on-demand (TTL = 15 min para cripto, 1h para ações fora do horário).

## Critério de pronto (DoD)
- Cadastrar manualmente posição em BTC, ETH, ADA, SOL e ver lucro/prejuízo recalculado.
- Mock de cotação retorna valores parametrizáveis para teste.
- Listagem de posições agregada por tipo.
- Testes >= 80%.
