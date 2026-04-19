# Documentation Writing Boundaries

## Purpose

This file defines strict boundaries for updating project docs during implementation chats.

## Core Rules

- Keep completed tasks concise. One short result line is preferred.
- Do not add technical implementation details into completed todo items.
- Detailed planning for upcoming tasks is allowed when it helps future implementation.
- High-level planning in an old chat is allowed.
- Detailed planning of a new feature in an old chat should be moved to a new chat.

## Non-Negotiable Separation

- `docs/domain.md` and `docs/domain/*` are for business meaning only.
- Domain docs must not describe code structures, storage schemas, DTO fields, handlers, event pipelines, or other technical implementation choices.
- If a note explains "how it is implemented", it belongs outside domain docs.

## By Document Type

### `docs/todo.md`

- Completed items: concise outcomes only.
- Avoid post-factum technical narratives.
- Planned items may include moderate detail if it is actionable.

### `docs/domain.md`

- Keep only product/domain meaning and business rules.
- Do not include UI structure, technical steps, code-level implementation notes, or chat transcript details.

### `docs/domain/*`

- Apply the same boundary as `docs/domain.md`: business rules only.
- Allowed: gameplay semantics, domain terms, constraints, and expected outcomes.
- Not allowed: class names, table/field schemas, API contract details, mapper/storage notes, runtime pipeline internals.

### `docs/architecture.md`

- Primary place for implementation-level notes and current technical state.
- Record where data is stored, how modules interact, and staged implementation status.

### `docs/decisions.md`

- For tactical technical choices where trade-offs exist or the choice could be revisited.
- Each entry: decision, rationale (1–2 lines), scope, status.
- Not for: business rules, code style, or minor implementation details that affect only one file.

### `docs/features/*.md`

- One file per feature. Created before coding starts; updated as implementation progresses.
- Contains: acceptance criteria, domain references, architecture contract, phased implementation steps, rejected paths, open questions.
- Reference domain docs; do not duplicate their content.
- Not for: business rules (those belong in `docs/domain/`) or structural module descriptions (those belong in `docs/architecture.md`).

### `docs/ai/*`

- Use for agent process rules, writing boundaries, and chat workflow constraints.
- Do not duplicate business rules from domain docs unless needed as a short pointer.

## Escalation Rule

If a documentation update starts expanding into architecture design or detailed implementation planning for a new feature line, stop and continue in a new chat.

## Maintenance

Update this file when recurring documentation mistakes are identified.
