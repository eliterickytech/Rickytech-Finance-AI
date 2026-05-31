# Sprint 11 - Spec técnica (Frontend Integrações + Notícias)

## Tela Binance

```
┌──────────────────────────────────────────────────────────────────────┐
│  Binance                                            [● Conectada ]    │
├──────────────────────────────────────────────────────────────────────┤
│  API Key:    ****-****-****-XXXX            [ Revelar ]               │
│  API Secret: ****-****-****-****            [ Revelar ]               │
│                                                                      │
│  [ Testar conexão ]   [ Sincronizar agora ]                          │
│                                                                      │
│  Última sync: 2026-08-15 14:32                                       │
├──────────────────────────────────────────────────────────────────────┤
│  Últimas operações importadas                                         │
│  Data       | Side | Símbolo | Quantidade | Preço | Total            │
└──────────────────────────────────────────────────────────────────────┘
```

## Tela OpenFinance

Fluxo:
```
[1] Selecionar banco (Itaú / Bradesco / Nubank / Inter / BTG / XP / Outros)
[2] Clicar "Iniciar consentimento"
[3] Redirect → sandbox/banco
[4] Banco redireciona para /integracoes/openfinance/callback?code=...
[5] Frontend chama backend, mostra spinner
[6] Resultado: "X transações importadas"
```

## Painel de Notícias

```
┌─────────────────────────────────────────────────────────────────────────┐
│  Notícias                                       [ Buscar... ]            │
├──────────────────────────┬──────────────────────────────────────────────┤
│  Filtros                 │  ┌─────────────────────────────────────┐    │
│  ◉ Todas                 │  │ [img] Ethereum hits new high...     │    │
│  ○ Cripto                │  │       CoinDesk · 2h atrás · #ETH    │    │
│  ○ Financeiro BR         │  └─────────────────────────────────────┘    │
│  ○ Internacional         │  ┌─────────────────────────────────────┐    │
│                          │  │ [img] Selic mantida em 10.5%...      │    │
│  Tags                    │  │       InfoMoney · 4h atrás · #Selic │    │
│  [BTC] [ETH] [ADA]       │  └─────────────────────────────────────┘    │
│  [SOL] [IBOV] [Selic]    │                                              │
│                          │  ... (infinite scroll)                       │
└──────────────────────────┴──────────────────────────────────────────────┘
```

## Endpoints consumidos

| Endpoint                                                | Tela              |
|---------------------------------------------------------|-------------------|
| `POST /api/v1/integracoes/binance`                      | Binance           |
| `POST /api/v1/integracoes/binance/sync`                 | Binance           |
| `GET  /api/v1/integracoes/binance/status`               | Binance           |
| `POST /api/v1/integracoes/openfinance/consentir`        | OpenFinance       |
| `GET  /api/v1/integracoes/openfinance/callback`         | OpenFinance       |
| `POST /api/v1/integracoes/openfinance/sync`             | OpenFinance       |
| `GET  /api/v1/noticias?category=...&tag=...&search=...` | Notícias          |
