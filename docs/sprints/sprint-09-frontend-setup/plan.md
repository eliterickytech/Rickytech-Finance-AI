# Sprint 9 - Frontend Setup + Color Admin

## Objetivo
Subir o projeto React + TypeScript + Vite, integrar o template **Color Admin**
do SeanThemes **à risca** (sidebar fixa, header escuro, painel dark + variantes).

## Escopo IN
- Estrutura base já criada (Sprint 0 do frontend).
- Copiar os assets do Color Admin para `src/theme/color-admin/`:
  - `scss/` (variables, mixins, components, dashboards)
  - `images/` (logos, ícones do tema)
  - `fonts/` (Open Sans, etc.)
  - `js/` (apenas o que for útil — preferimos reescrever em React).
- Configurar Bootstrap 5 + SCSS do Color Admin como entrypoint.
- Implementar **MainLayout**: Sidebar + Header + Footer + TopMenu.
- Toggle de **tema escuro** (default) e **claro**.
- Rotas placeholder para todas as features (já listadas em `routes/AppRoutes.tsx`).
- i18n PT-BR / EN-US.
- Configurar cliente HTTP (axios) com interceptors e baseURL.
- ESLint + Prettier + Vitest.

## Escopo OUT
- Telas reais de CRUD (Sprint 10).
- Notícias / Integrações (Sprint 11).

## Decisões
- Variante padrão do Color Admin: **Default Black** (dark sidebar + dark header).
- Tipografia: **Inter** ou **Open Sans** conforme o tema.
- Ícones: **Font Awesome 6** (incluído no pacote do Color Admin).
- Gerenciamento de estado: Redux Toolkit + RTK Query (para CRUDs).

## Critério de pronto (DoD)
- Subir `npm run dev` e ver o layout Color Admin idêntico ao demo
  (sidebar escura à esquerda com itens de menu, header escuro, breadcrumb,
  área central com cards placeholder).
- Tema escuro/claro alternando sem perda de estado.
- Navegação entre rotas placeholder funciona.
- Idioma alterna entre PT-BR e EN-US.
