# Sprint 11 - Frontend: Integrações e Notícias

## Objetivo
Construir as telas de **Integrações** (Binance + OpenFinance) e o painel de
**Notícias** financeiras/cripto.

## Escopo IN
- **Integrações Binance** (`/integracoes/binance`):
  - Card com status (Conectada / Desconectada / Erro)
  - Form para entrada de API Key + Secret (mascarados)
  - Botão "Testar conexão"
  - Botão "Sincronizar agora" + último sync
  - Tabela com últimas 50 operações importadas
- **Integrações Open Finance** (`/integracoes/openfinance`):
  - Seleção de banco
  - Botão "Iniciar consentimento" → redireciona para sandbox
  - Callback handler (espera redirect → mostra "Sincronizando...")
  - Histórico de syncs
- **Notícias** (`/noticias`):
  - Layout estilo "newsroom" baseado no Inbox do Color Admin
  - Filtros: categoria (Crypto / Financial BR / Internacional) + tags
  - Busca textual
  - Cartão de notícia com imagem, fonte, data, summary
  - Click abre em nova aba

## Decisões
- Chaves Binance **nunca** aparecem em GET (só placeholder `****`).
- Painel de notícias com infinite scroll.

## Critério de pronto (DoD)
- Tela Binance permite cadastrar chaves, testar e sincronizar.
- Tela OpenFinance executa fluxo de consent com mock-server.
- Painel de notícias mostra ao menos 50 itens reais do backend.
