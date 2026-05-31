# Sprint 8 - Notícias Financeiras e Cripto

## Objetivo
Entregar um agregador de notícias que alimenta uma aba no app com manchetes
do mercado tradicional e cripto, atualizadas a cada 15 minutos.

## Escopo IN
- Entidade `NewsItem` (cache local de feeds).
- `INewsAggregator` em Application; implementação em Infrastructure consome RSS.
- Fontes default:
  - **Cripto**: CoinDesk, CoinTelegraph, Decrypt, The Block, Brave New Coin
  - **Financeiro BR**: InfoMoney, Valor Econômico, ADVFN, Suno Notícias
  - **Internacional**: Reuters Finance, Bloomberg, Yahoo Finance
- Endpoint `GET /api/v1/noticias?category=Crypto&search=ethereum&page=...`
- Background job (HostedService): refresh a cada 15 min.
- Tags automáticas: BTC, ETH, ADA, SOL, BNB, IBOV, S&P, FED, Selic, IPCA.

## Escopo OUT
- Notícias em vídeo (futuro).
- Análise de sentimento por IA (futuro).

## Decisões
- RSS direto > APIs pagas no MVP.
- Deduplicação por hash da URL.
- TTL de 7 dias para a tabela de notícias (limpeza diária).

## Critério de pronto (DoD)
- Endpoint retorna 50 notícias atuais por categoria.
- Background job preenche o cache no startup.
- Testes com feeds mockados (XML fixos em testfixtures).
