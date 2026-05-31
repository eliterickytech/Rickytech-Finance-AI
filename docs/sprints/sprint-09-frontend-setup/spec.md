# Sprint 9 - Spec técnica (Frontend Setup)

## Estrutura

```
frontend/src/
├── assets/scss/app.scss              # entrypoint que importa color-admin
├── theme/color-admin/
│   ├── _variables.scss
│   ├── _mixins.scss
│   ├── _sidebar.scss
│   ├── _header.scss
│   ├── _content.scss
│   ├── _widgets.scss
│   ├── _dashboard.scss
│   └── index.scss
├── components/layout/
│   ├── MainLayout.tsx                # composição: Sidebar+Header+Content+Footer
│   ├── Sidebar/Sidebar.tsx
│   ├── Header/Header.tsx
│   ├── Footer/Footer.tsx
│   └── TopMenu/TopMenu.tsx
├── hooks/
│   ├── useTheme.ts                   # dark/light
│   └── useBreadcrumb.ts
└── locales/
    ├── pt-BR/common.json
    └── en-US/common.json
```

## Sidebar (menu)

```ts
const menu = [
  { key: 'dashboard',    label: 'Dashboard',     icon: 'fa-tachometer-alt', to: '/dashboard' },
  { key: 'cadastros',    label: 'Cadastros',     icon: 'fa-folder',
    children: [
      { key: 'categorias', label: 'Categorias', to: '/categorias' },
      { key: 'bancos',     label: 'Bancos',     to: '/bancos' },
    ]
  },
  { key: 'movimentacao', label: 'Movimentação',  icon: 'fa-exchange-alt',
    children: [
      { key: 'receitas', label: 'Receitas', to: '/receitas' },
      { key: 'despesas', label: 'Despesas', to: '/despesas' },
    ]
  },
  { key: 'investimentos', label: 'Investimentos', icon: 'fa-chart-line', to: '/investimentos' },
  { key: 'projecoes',     label: 'Projeções',     icon: 'fa-magic',      to: '/projecoes' },
  { key: 'integracoes',   label: 'Integrações',   icon: 'fa-plug',
    children: [
      { key: 'binance',     label: 'Binance',      to: '/integracoes/binance' },
      { key: 'openfinance', label: 'Open Finance', to: '/integracoes/openfinance' },
    ]
  },
  { key: 'noticias',      label: 'Notícias',      icon: 'fa-newspaper',  to: '/noticias' },
  { key: 'configuracoes', label: 'Configurações', icon: 'fa-cog',        to: '/configuracoes' },
];
```

## Header

- Logo "Finance Control"
- Search global
- Idioma (PT/EN)
- Tema (escuro/claro)
- Perfil (foto + dropdown: Minha conta / Configurações / Sair)
- Notificações

## Aderência ao Color Admin

| Componente Color Admin | Em qual página             | Sprint |
|------------------------|----------------------------|--------|
| Sidebar dark           | Todas                      | 9      |
| Header dark            | Todas                      | 9      |
| Stat widget            | Dashboard                  | 10     |
| Pie / Bar chart        | Dashboard / Projeções      | 10     |
| Table avançada         | CRUDs                      | 10     |
| Form layout            | CRUDs                      | 10     |
| Inbox / Mailbox layout | Notícias (reaproveitando)  | 11     |
