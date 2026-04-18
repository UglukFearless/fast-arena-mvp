# AI Pitfalls

## Purpose

This file captures frequent mistakes and constraints relevant to this project.

## Known Pitfalls

- **Always read `docs/ai/pitfalls.md` at the start of any task.** Known constraints live here; skipping this causes repeated mistakes.
- Do not treat roadmap items as current implementation facts.
- Do not infer gameplay mechanics beyond `docs/domain.md`.
- Mapping depth can affect nested response fields; verify related mappings when changing DTO shaping.
- Keep MVP assumptions explicit; future ideas are not current behavior.
- **Do NOT generate, run, or commit EF migrations.** Migrations are disabled in MVP. Schema changes go directly in `ApplicationContext.OnModelCreating()`. See `docs/architecture.md` for the MVP migration policy.
- Schema changes are applied by manually recreating the database locally. Do not auto-drop, auto-create, or auto-reset the database.

## Documentation Boundaries

- Single source of truth for documentation granularity and chat boundaries: `docs/ai/doc-writing-boundaries.md`.
- `docs/ai/pitfalls.md` should not duplicate those rules.

## Maintenance

Add short entries after real incidents, regressions, or repeated confusion.