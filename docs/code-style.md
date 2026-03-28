# Code Style

## Purpose

This document stores project-specific code style decisions that are important for readability and team consistency.

It is intentionally manual and incremental. Add rules when they become important in real work.

## Principles

- Readability is more important than brevity.
- Visual structure should match execution structure.
- Team conventions are more important than personal habits inside this repository.
- If a style rule improves clarity in this codebase, prefer the explicit form.

## Current Rules

### Single-Line `if`

- Do not write ordinary `if` statements as a single-line statement like `if (condition) return value;`.
- In normal flow, use the full block form with braces.
- Exception: Use if (condition) without braces only for early exits spanning two lines.

Preferred:

```ts
if (Condition) {
    // some operations
    return value;
}
```

```ts
if (isInvalid)
    return null;
```

Avoid:

```ts
if (isInvalid) return null;
```

### Comment Language

- Comments inside code must be written in English.
- Project documentation may be written in Russian.

## How To Extend This File

When adding a new rule, prefer this format:

1. Rule name
2. Short rationale
3. Preferred form
4. Optional exception

## Change Policy

Update this file when repeated style remarks appear during review or implementation.