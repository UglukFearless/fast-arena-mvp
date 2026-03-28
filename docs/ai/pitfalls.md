# AI Pitfalls

## Purpose

This file captures frequent mistakes and constraints relevant to this project.

## Known Pitfalls

- Do not treat roadmap items as current implementation facts.
- Do not infer gameplay mechanics beyond `docs/domain.md`.
- Mapping depth can affect nested response fields; verify related mappings when changing DTO shaping.
- Keep MVP assumptions explicit; future ideas are not current behavior.

## Documentation Boundaries

- Canonical meaning belongs in `docs/domain.md` and `docs/architecture.md`.
- `docs/ai/*` should route and constrain, not duplicate product documentation.

## Maintenance

Add short entries after real incidents, regressions, or repeated confusion.