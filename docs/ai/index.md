# AI Routing Index

## Purpose

This file defines what an AI agent should read first, depending on task type.
Canonical business knowledge lives in human documentation under `docs/`.

## Reading Order By Task

### Domain Or Business Rules

1. Read `docs/ai/pitfalls.md` first.
2. Read `docs/domain.md` (index) and the relevant sub-page(s) under `docs/domain/`.
3. If implementation details are needed, read `docs/architecture.md`.
4. For near-term execution context, read `docs/todo.md`.
5. If domain information is missing or ambiguous, follow `docs/ai/domain-gaps.md`.

### Feature Implementation

1. Read `docs/ai/pitfalls.md` first.
2. Read `docs/domain.md` (index) and the relevant sub-page(s) under `docs/domain/`.
3. Read `docs/architecture.md`.
4. Read `docs/code-style.md`.
5. Read `docs/todo.md`.
6. Read `docs/decisions.md` for any prior decisions that affect this feature.
7. If an implementation note exists, read `docs/features/{feature-name}.md`.
8. If direction trade-offs matter, read `docs/roadmap.md`.

### Planning And Prioritization

1. Read `docs/ai/pitfalls.md` first.
2. Read `docs/roadmap.md`.
3. Read `docs/todo.md`.
4. Read `docs/domain.md` to validate product meaning.

### Documentation Updates

1. Read `docs/ai/pitfalls.md` first.
2. Read `docs/ai/doc-writing-boundaries.md`.
3. Read target canonical doc (`docs/domain.md`, `docs/todo.md`, `docs/architecture.md`, etc.).
4. Keep updates proportional to the target document scope.

### Decisions And Rationale

1. Read `docs/decisions.md` for the log of tactical technical decisions.
2. If the decision is feature-specific, also read `docs/features/{feature-name}.md` for full context.

### Environment And Runtime Commands

1. Read `docs/ai/commands.md`.
2. If needed, confirm top-level context in `README.md`.

## Working Rules

- Treat `docs/domain.md` as source of truth for product semantics.
- Keep `docs/domain.md` and `docs/domain/*` business-only; put implementation details into `docs/architecture.md`.
- Treat `docs/code-style.md` as source of truth for project-specific style rules.
- Treat `docs/ai/doc-writing-boundaries.md` as source of truth for documentation granularity and chat boundaries.
- Implementation plans and phased steps live in `docs/features/`. Create one note per feature before coding starts.
- Tactical decisions live in `docs/decisions.md`. Add an entry when a non-obvious technical choice is made.
- Flow: domain rules → decisions → feature note → code.
- Do not invent business rules that are absent from docs.
- If docs are ambiguous, ask for clarification before changing behavior.
- Keep outputs consistent with current MVP scope.

## Maintenance

Update this file when documentation structure changes.