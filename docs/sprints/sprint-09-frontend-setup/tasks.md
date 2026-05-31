# Sprint 9 - Tasks

> Estimativa: **29 pts**
>
> _Checkboxes atualizadas em 2026-05-31 conforme auditoria do código-fonte._

## US-S9-01 - Importar assets do Color Admin (5 pts)
- [x] Copiar `scss/` para `src/theme/color-admin/`
- [x] Copiar `images/` e `fonts/`
- [x] Ajustar imports no `assets/scss/app.scss`

## US-S9-02 - MainLayout (8 pts)
- [x] Componente `MainLayout` com Sidebar + Header + Footer + TopMenu
- [x] Sidebar collapsible (responsivo mobile)
- [x] Top profile + notificações + search

## US-S9-03 - Tema escuro/claro (3 pts)
- [x] Toggle persistido em localStorage
- [x] Atributo `data-bs-theme` no `<html>`

## US-S9-04 - Roteamento + breadcrumbs (3 pts)
- [x] Atualizar `AppRoutes.tsx` para usar `MainLayout`
- [x] Breadcrumb automático baseado na rota

## US-S9-05 - i18n PT-BR / EN-US (3 pts)
- [x] Configurar `react-i18next`
- [x] Dicionários para sidebar e títulos de página (locales `pt-BR` / `en-US`)

## US-S9-06 - Axios + interceptors (3 pts)
- [x] httpClient com Bearer token
- [x] Handler de 401 → logout

## US-S9-07 - ESLint + Prettier + Vitest (2 pts)
- [x] Configurar ESLint + Vitest (script `lint` e `test` no `package.json`) — _Prettier não encontrado_
- [ ] Adicionar smoke test do `App.tsx` — **nenhum teste de frontend escrito**

## US-S9-08 - Storybook (opcional - 2 pts)
- [ ] Subir Storybook para componentes do MainLayout — **não configurado**
