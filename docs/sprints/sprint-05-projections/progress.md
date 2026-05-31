# Sprint 5 - Progress

> **Legenda:** ✅ Implementado · 🟡 Parcial · ❌ Pendente

## Diário

### 2026-05-31 (Auditoria de código)
- Projeção implementada via `RunProjectionCommand` + `ProjectionDtos` + `ProjecoesController`.
- 🟡 A lógica (cenários/juros) vive **dentro do handler do command**, não há um `IProjectionService` dedicado nem pasta de serviço.
- ❌ Sem cache (`IMemoryCache`) nem idempotência implementados.
- ❌ Pasta `Validators/` de Projections está vazia.
- ❌ Sem testes específicos de projeção.
- ⚠️ Migration ainda não gerada (caso a projeção persista cenários).

## Status das US

| US                                       | Status          |
|------------------------------------------|-----------------|
| US-S5-01 IProjectionService              | 🟡 Parcial (lógica no command, sem service dedicado) |
| US-S5-02 Modelagem de cenário            | ✅ Implementado |
| US-S5-03 Juros compostos                 | ✅ Implementado (no handler) |
| US-S5-04 Command + Query                 | ✅ Implementado |
| US-S5-05 Controller                      | ✅ Implementado |
| US-S5-06 Cache + Idempotência            | ❌ Pendente |
| US-S5-07 Testes                          | ❌ Pendente |

## Bloqueios
_(nenhum crítico; refatorar lógica para um serviço dedicado é desejável)_

## Demo prevista
- Projetar 12 meses em cenário realista para os bancos cadastrados
- Comparar gráfico otimista vs pessimista lado a lado
