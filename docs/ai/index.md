# AI Routing Index

## Purpose

This file defines what an AI agent should read first, depending on task type.
Canonical business knowledge lives in human documentation under `docs/`.

## Reading Order By Task

### Domain Or Business Rules

1. Read `docs/domain.md` (index) and the relevant sub-page(s) under `docs/domain/`.
2. If implementation details are needed, read `docs/architecture.md`.
3. For near-term execution context, read `docs/todo.md`.
4. If domain information is missing or ambiguous, follow `docs/ai/domain-gaps.md`.

### Feature Implementation

1. Read `docs/domain.md` (index) and the relevant sub-page(s) under `docs/domain/`.
2. Read `docs/architecture.md`.
3. Read `docs/code-style.md`.
4. Read `docs/todo.md`.
5. If direction trade-offs matter, read `docs/roadmap.md`.

### Planning And Prioritization

1. Read `docs/roadmap.md`.
2. Read `docs/todo.md`.
3. Read `docs/domain.md` to validate product meaning.

### Documentation Updates

1. Read `docs/ai/doc-writing-boundaries.md`.
2. Read target canonical doc (`docs/domain.md`, `docs/todo.md`, `docs/architecture.md`, etc.).
3. Keep updates proportional to the target document scope.

### Environment And Runtime Commands

1. Read `docs/ai/commands.md`.
2. If needed, confirm top-level context in `README.md`.

## Working Rules

- Treat `docs/domain.md` as source of truth for product semantics.
- Treat `docs/code-style.md` as source of truth for project-specific style rules.
- Treat `docs/ai/doc-writing-boundaries.md` as source of truth for documentation granularity and chat boundaries.
- Do not invent business rules that are absent from docs.
- If docs are ambiguous, ask for clarification before changing behavior.
- Keep outputs consistent with current MVP scope.

## Maintenance

Update this file when documentation structure changes.