# Especificação Técnica Global - Finance Control

## 1. Requisitos funcionais (RF)

### RF-01 - Categorias
- CRUD de categorias.
- Cada categoria pertence a um **tipo**: `Receita`, `Despesa` ou `Ambos`.
- Suportar hierarquia opcional (categoria-pai → subcategoria).
- Campos: `Id`, `Nome`, `Tipo`, `Icone`, `Cor`, `CategoriaPaiId?`, `Ativo`.

### RF-02 - Bancos
- CRUD de bancos / contas.
- Tipos suportados: `ContaCorrente`, `Poupanca`, `Carteira`, `Corretora`, `Cripto`.
- Campos: `Id`, `Nome`, `Apelido`, `Tipo`, `Agencia?`, `Conta?`, `SaldoAtual`, `MoedaPadrao` (BRL, USD, USDT, BTC, ...), `Ativo`.
- Saldo é calculado por soma de receitas - despesas + ajustes.

### RF-03 - Receitas
- CRUD de receitas.
- Campos: `Id`, `Descricao`, `Valor`, `Data`, `CategoriaId`, `BancoId`, `Recorrente`, `Frequencia` (Mensal, Quinzenal, ...), `Tags[]`, `Anexo?`, `IntegracaoOrigemId?`.
- Suporte a recorrência (gera lançamentos futuros).
- Sem dependência circular: ao deletar Categoria/Banco, bloquear se houver lançamentos vinculados.

### RF-04 - Despesas
- CRUD de despesas (espelhamento das receitas).
- Adicional: `MetodoPagamento` (Pix, Cartão, Débito, Dinheiro, ...).

### RF-05 - Investimentos
- CRUD de investimentos.
- Tipos: `RendaFixa` (CDB, Tesouro), `RendaVariavel` (Ações, ETFs, FIIs), `Cripto` (BTC, ETH, ADA, SOL, ...).
- Campos: `Id`, `Ativo` (ticker / símbolo), `Tipo`, `Quantidade`, `PrecoMedio`, `DataAquisicao`, `BancoId`, `Notas`.
- Cálculo automático de **valor de mercado** (cotação atual) e **lucro/prejuízo**.

### RF-06 - Integrações
- CRUD de **configurações** de integração (chaves API por usuário).
- **Binance**: importar trades, depósitos/saques, posições.
- **Open Finance Brasil**: importar extratos via consentimento OFI.
- **Bancos** (corretoras / contas correntes): importar via OFI.
- Ao concluir uma integração, **disparar pipeline de ingestão** que cria receitas/despesas/investimentos automaticamente.

### RF-07 - Banco de dados local
- SQL relacional (default: SQL Server LocalDB; alternativa: SQLite).
- Migrations via EF Core 10.

### RF-08 - Projeções de lucros
- Calcular fluxo de caixa projetado para `N` meses (default 12).
- Inputs: receitas recorrentes + despesas recorrentes + investimentos com taxa esperada.
- Outputs: saldo projetado mensal, gráfico de evolução, cenário otimista / pessimista / realista.

### RF-09 - Notícias
- Aba dedicada agregando RSS:
  - **Cripto**: CoinDesk, CoinTelegraph, Decrypt.
  - **Financeiro tradicional**: InfoMoney, Valor Econômico, Investing.com.
- Filtros por categoria, busca textual, favoritos.

## 2. Requisitos não-funcionais (RNF)

| ID | Requisito |
|----|-----------|
| RNF-01 | Backend em **C# .NET 10** |
| RNF-02 | **Clean Architecture** + **CQRS** com MediatR |
| RNF-03 | API em **Minimal API** com Controllers, projeto separado, somente endpoints |
| RNF-04 | Validação de requests com **FluentValidation** |
| RNF-05 | **AutoMapper** com profiles na camada Application |
| RNF-06 | **Interfaces** ficam na camada Application |
| RNF-07 | **CrossCutting** centraliza DI (separadamente para Application e Infrastructure) |
| RNF-08 | **Middlewares** ficam na API, pasta `Middlewares/` |
| RNF-09 | `BaseApiController` padroniza responses (`ApiResponse<T>`) |
| RNF-10 | **Serilog fortemente tipado**, sinks Console + File |
| RNF-11 | Frontend em **React** seguindo template **Color Admin** do SeanThemes à risca |
| RNF-12 | Cobertura de testes ≥ 70% nas camadas Application e Domain |
| RNF-13 | Tempo de resposta médio < 300 ms nos CRUDs (sem rede externa) |
| RNF-14 | Logs sempre com `CorrelationId` por request |

## 3. Entidades do domínio (mapa inicial)

```
User (1) ──────────┐
                   ▼
   Category    Bank    Investment    IntegrationConfig
       ▲         ▲          ▲
       │         │          │
       └─Income──┘          │
       └─Expense─┘          │
                            │
                       AssetQuote (cache de cotações)
```

## 4. Endpoints (visão consolidada)

| Recurso         | Endpoint base                          |
|-----------------|-----------------------------------------|
| Categorias      | `/api/v1/categorias`                    |
| Bancos          | `/api/v1/bancos`                        |
| Receitas        | `/api/v1/receitas`                      |
| Despesas        | `/api/v1/despesas`                      |
| Investimentos   | `/api/v1/investimentos`                 |
| Integrações     | `/api/v1/integracoes`                   |
| Binance         | `/api/v1/integracoes/binance/{...}`     |
| OpenFinance     | `/api/v1/integracoes/openfinance/{...}` |
| Projeções       | `/api/v1/projecoes`                     |
| Notícias        | `/api/v1/noticias`                      |
| Health          | `/health`                               |
| Swagger         | `/swagger`                              |

## 5. Envelope padrão de resposta

```json
{
  "success": true,
  "data": { /* T */ },
  "message": "Operação realizada com sucesso.",
  "errors": null,
  "timestamp": "2026-05-25T14:00:00Z"
}
```

Em erro:

```json
{
  "success": false,
  "data": null,
  "message": "Validação falhou.",
  "errors": ["Nome é obrigatório.", "Tipo deve ser Receita ou Despesa."],
  "timestamp": "2026-05-25T14:00:00Z"
}
```

## 6. Critérios de aceite globais

- Toda nova **feature** entra através do MediatR (Command/Query + Handler + Validator).
- Todo handler que muda estado **valida** via FluentValidation **antes** de tocar no DbContext.
- Todo response da API segue o envelope `ApiResponse<T>`.
- Toda exception não tratada é capturada pelo `ExceptionHandlingMiddleware`.
- Toda request tem `X-Correlation-Id` no header de saída.
- Toda Migration EF Core é versionada por sprint (nome: `YYYYMMDD_HHMM_Sprint0X_<descrição>`).
