# Sprint 10 - Frontend CRUDs e Dashboard

## Objetivo
Construir todas as telas de CRUD (Categorias, Bancos, Receitas, Despesas,
Investimentos) e o Dashboard principal com widgets e gráficos.

## Escopo IN
- **Dashboard** (`/dashboard`):
  - Stat widgets: saldo total, receitas no mês, despesas no mês, lucro líquido
  - Gráfico de evolução do saldo (ApexCharts area)
  - Gráfico despesas por categoria (pie)
  - Tabela "Próximas contas a pagar"
- **Categorias** (`/categorias`): list + create + edit + delete (modal)
- **Bancos** (`/bancos`): list (cards de saldo) + form completo
- **Receitas / Despesas**: list paginada com filtros + drawer de edição
- **Investimentos** (`/investimentos`): tabela com lucro/prejuízo + detalhe com gráfico de evolução
- **Projeções** (`/projecoes`): formulário (horizonte + cenário) + gráfico de linha

## Escopo OUT
- Notícias e telas de Integrações (Sprint 11).

## Decisões
- Forms com **React Hook Form + Yup**.
- Tabelas com paginação server-side via RTK Query.
- ApexCharts para todos os gráficos.

## Critério de pronto (DoD)
- Cada CRUD funcional ponta-a-ponta contra o backend.
- Dashboard exibe dados reais.
- Testes unitários dos hooks de RTK Query (mock service worker).
