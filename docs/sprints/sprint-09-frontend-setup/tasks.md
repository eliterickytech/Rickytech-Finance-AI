# Sprint 9 - Tasks

> Estimativa: **29 pts**

## US-S9-01 - Importar assets do Color Admin (5 pts)
- [ ] Copiar `scss/` para `src/theme/color-admin/`
- [ ] Copiar `images/` e `fonts/`
- [ ] Ajustar imports no `assets/scss/app.scss`

## US-S9-02 - MainLayout (8 pts)
- [ ] Componente `MainLayout` com Sidebar + Header + Footer + TopMenu
- [ ] Sidebar collapsible (responsivo mobile)
- [ ] Top profile + notificações + search

## US-S9-03 - Tema escuro/claro (3 pts)
- [ ] Toggle persistido em localStorage
- [ ] Atributo `data-bs-theme` no `<html>`

## US-S9-04 - Roteamento + breadcrumbs (3 pts)
- [ ] Atualizar `AppRoutes.tsx` para usar `MainLayout`
- [ ] Breadcrumb automático baseado na rota

## US-S9-05 - i18n PT-BR / EN-US (3 pts)
- [ ] Configurar `react-i18next`
- [ ] Dicionários para sidebar e títulos de página

## US-S9-06 - Axios + interceptors (3 pts)
- [ ] httpClient com Bearer token
- [ ] Handler de 401 → logout

## US-S9-07 - ESLint + Prettier + Vitest (2 pts)
- [ ] Configurar
- [ ] Adicionar smoke test do `App.tsx`

## US-S9-08 - Storybook (opcional - 2 pts)
- [ ] Subir Storybook para componentes do MainLayout
