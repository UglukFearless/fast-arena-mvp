# Todo

## Purpose

This document tracks short-horizon tasks. It should stay compact and operational.

## Current Priorities

### Documentation

- [ ] Fill `docs/architecture.md` with codebase-specific examples.

### Completed Goals

- **Hero Inventory** — shop, buy/sell, inventory in HeroInfo. Done.
- **Item Effects, Pockets, Combat Usage** — potions, pocket equipment, USE_ITEM action, effect indicators in fight UI. Done. See [`docs/features/item-effects.md`](features/item-effects.md).
- **Monster Item Rewards** — per-monster drop table, seeding, structured reward panel. Done. See [`docs/features/monster-item-rewards.md`](features/monster-item-rewards.md).

### Next Goal: Weapon Usage in Combat

Implementation note: [`docs/features/weapon-usage.md`](features/weapon-usage.md)

- [ ] Finalize domain rules for weapon equip slots and combat damage contribution.
- [ ] Define architecture contract for weapon effect application in fight service.
- [ ] Add DAL/domain support for equipping weapons in right/left hand slots.
- [ ] Implement weapon damage modifier application in `MonsterFightService`.
- [ ] Seed initial weapon items and expose them in the shop.
- [ ] Update fight UI to show equipped weapon(s).
- [ ] Add backend tests for weapon damage modifier logic.

### Technical Debt

- `MonsterFightRewardTests` — `AggregateStackableItems` is tested via reflection into a private service method. Revisit during service refactoring: extract the logic to a proper testable unit and replace the reflection-based test. See [`docs/features/monster-item-rewards.md`](features/monster-item-rewards.md).

## Task Format

When adding new tasks, prefer this structure:

- `[ ] task`
- optional note or link to related file/feature

## Change Policy

Update this file frequently. Keep it compact — move details to feature docs.