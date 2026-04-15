# AI Pitfalls

## Purpose

This file captures frequent mistakes and constraints relevant to this project.

## Known Pitfalls

- Do not treat roadmap items as current implementation facts.
- Do not infer gameplay mechanics beyond `docs/domain.md`.
- Mapping depth can affect nested response fields; verify related mappings when changing DTO shaping.
- Keep MVP assumptions explicit; future ideas are not current behavior.
- MVP database workflow currently runs without migrations.
- Database reset/recreate is a manual developer action; the agent must not auto-drop or auto-reset the database.

## Documentation Boundaries

- Single source of truth for documentation granularity and chat boundaries: `docs/ai/doc-writing-boundaries.md`.
- `docs/ai/pitfalls.md` should not duplicate those rules.

## Maintenance

Add short entries after real incidents, regressions, or repeated confusion.