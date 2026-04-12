# Domain Gaps

## Purpose

This file defines how to handle situations where domain knowledge is missing or ambiguous during implementation.

## When Domain Information Is Insufficient

If the current task or existing documentation does not fully explain the business meaning of an entity, rule, or behavior:

1. **Ask clarifying questions before implementing.** Do not invent rules or fill gaps with assumptions.
2. **Be specific.** Ask about the exact aspect that is unclear — a rule, an entity relationship, an edge case.
3. **After getting the answer, propose to update the relevant domain sub-page.** Point to the specific file in `docs/domain/`.

## Domain Sub-Pages

| Topic | File |
|---|---|
| Product idea, core loop, domain rules | [../domain/overview.md](../domain/overview.md) |
| Hero: characteristics, progression, ownership | [../domain/hero.md](../domain/hero.md) |
| Combat: monster, fight, rewards and penalties | [../domain/combat.md](../domain/combat.md) |
| Items: types and behavioral flags | [../domain/items.md](../domain/items.md) |
| Economy: inventory, shop, transactions | [../domain/economy.md](../domain/economy.md) |

## Maintenance

Update this file when the policy for handling domain ambiguity changes.
