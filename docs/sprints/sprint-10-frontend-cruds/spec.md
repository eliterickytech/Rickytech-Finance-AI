# Sprint 10 - Spec técnica (Frontend CRUDs + Dashboard)

## Dashboard - Widgets

```
┌─ Saldo Total ─────┐ ┌─ Receitas (mês) ──┐ ┌─ Despesas (mês) ──┐ ┌─ Lucro Líquido ───┐
│  R$ 42.350,00     │ │  R$ 12.000,00     │ │  R$  8.500,00     │ │  R$  3.500,00     │
└───────────────────┘ └───────────────────┘ └───────────────────┘ └───────────────────┘

┌─ Evolução do Saldo (12 meses) ──────────┐ ┌─ Despesas por Categoria ────────┐
│  ApexCharts area (mensal)               │ │  ApexCharts pie                  │
└─────────────────────────────────────────┘ └──────────────────────────────────┘

┌─ Próximas Contas a Pagar ───────────────────────────────────────────────────┐
│  Tabela: Data | Descrição | Categoria | Valor | Status                       │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Padrão de tela CRUD

1. **Listagem**:
   - Header com botão "+ Novo"
   - Filtros (chips)
   - Tabela com paginação (50/página)
   - Ações inline: Editar / Duplicar / Excluir

2. **Formulário** (modal ou drawer):
   - React Hook Form + Yup
   - Loading + erros amigáveis
   - Submit otimista (RTK Query `optimisticUpdate`)

## Detalhe de Investimento

```
[Header: Ticker | Tipo | Quantidade | Preço Médio | Valor Atual | L/P]
[Gráfico de cotação (ApexCharts line, 90 dias)]
[Tabela de operações (Buy/Sell/Fee)]
```

## Tela Projeções

```
Form:
  - Horizonte (meses): 1..60 (default 12)
  - Cenário: Otimista / Realista / Pessimista
  - Inflação anual (%) (default 4.5)

Resultado:
  - Card de resumo (saldo final, ROI, total investido, lucro líquido)
  - Gráfico de área (3 séries lado a lado quando comparando cenários)
```

## RTK Query - exemplo Categorias

```ts
export const categoriesApi = createApi({
  reducerPath: 'categoriesApi',
  baseQuery: axiosBaseQuery({ baseUrl: '/api/v1/categorias' }),
  tagTypes: ['Category'],
  endpoints: (b) => ({
    list:   b.query<CategoryDto[], CategoryFilter>({ ..., providesTags: ['Category'] }),
    get:    b.query<CategoryDto, string>({ ... }),
    create: b.mutation<CategoryDto, CreateCategoryDto>({ ..., invalidatesTags: ['Category'] }),
    update: b.mutation<CategoryDto, UpdateCategoryDto>({ ..., invalidatesTags: ['Category'] }),
    remove: b.mutation<void, string>({ ..., invalidatesTags: ['Category'] }),
  }),
});
```
