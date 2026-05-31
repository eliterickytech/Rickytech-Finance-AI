# Sprint 7 - Integração Open Finance Brasil / Bancos

## Objetivo
Importar extratos bancários via **Open Finance Brasil** (OFI) ou conectores
diretos com bancos parceiros, criando receitas/despesas no Finance Control.

## Escopo IN
- Fluxo OAuth 2.0 + DCR (Dynamic Client Registration) simplificado para sandbox.
- Provider abstrato `IOpenFinanceClient` em Application.
- Implementação inicial usando **sandbox do Banco Central** (consentimento + accounts + transactions).
- Endpoint `POST /api/v1/integracoes/openfinance/consentir` → retorna URL de consentimento.
- Callback `GET /api/v1/integracoes/openfinance/callback?code=...&state=...`
- Endpoint `POST /api/v1/integracoes/openfinance/sync` → puxa transações.
- Mapeamento: cada transação OFI vira `Income` (CREDIT) ou `Expense` (DEBIT) com `IntegrationSourceId = "OFI:<transactionId>"`.
- Categorização automática simples por keyword (Pix, Boleto, Salário, Supermercado, ...) — base para ML futuro.

## Escopo OUT
- Pagamento de boletos via OFI (futuro).
- Onboarding completo de certificados ICP-Brasil (produção real).

## Decisões
- Em dev, usar **sandbox** público (`https://matls-api.sandbox.directory.openbankingbrasil.org.br`) com mock própria se sandbox indisponível.
- Tokens curtos: salvar `AccessToken`, `RefreshToken`, `ExpiresAt` criptografados em `IntegrationConfig.Metadata`.

## Riscos
| Risco                                     | Mitigação                              |
|-------------------------------------------|------------------------------------------|
| Sandbox instável                          | Modo "mock-server" embutido              |
| Complexidade DCR                          | Adiar produção real para pós-MVP         |

## Critério de pronto (DoD)
- Fluxo end-to-end no sandbox: consentir → callback → sync → ver transações importadas
- Testes com mock-server (WireMock.Net) simulando endpoints OFI
- Documentar limites de produção (necessita certificado ICP-Brasil etc.)
