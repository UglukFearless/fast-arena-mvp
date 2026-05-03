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

### Completed Goals (continued)

- **Weapon And Shield Usage In Combat (backend)** — equipment effect pipeline, persistent effect lifecycle, shield block phase, weapon damage modifier, shop expanded to WEAPON/SHIELD, equip/unequip via `HeroEquipmentService`, seed bindings, backend tests. Done. See [`docs/features/weapon-usage.md`](features/weapon-usage.md).
- **Weapon And Shield Usage In Combat (frontend)** — equip/unequip UI, slot pre-validation, shield block display, equipment panel in fight window, active effects panel, pocket items source moved to hero. Done. See [`docs/features/weapon-usage.md`](features/weapon-usage.md).

### Technical Debt

- `ItemDto.AllowedSlots` — currently the concept of "two-handed weapon" is implicit: an item is two-handed if and only if its `AllowedSlots` contains both `RIGHT_HAND` and `LEFT_HAND`. This is a structural assumption baked into both the backend service (`HeroEquipmentService.IsTwoHandedWeapon`) and the frontend slot-resolution logic. If slot rules become more complex (e.g., ambidextrous items, multi-slot armor), this implicit convention will break. Consider introducing an explicit `WeaponHandedness` enum or a dedicated `IsTwoHanded` domain property on `Item` that is resolved once at data definition time.

- `MonsterFightRewardTests` — `AggregateStackableItems` is tested via reflection into a private service method. Revisit during service refactoring: extract the logic to a proper testable unit and replace the reflection-based test. See [`docs/features/monster-item-rewards.md`](features/monster-item-rewards.md).
- `MonsterFightServiceLifecycleTests` — private methods of `MonsterFightService` are tested via reflection. This is a fragile testability vulnerability and couples tests to implementation details. Rework `MonsterFightService` by extracting fight lifecycle functionality into dedicated methods/units and cover behavior through stable test seams instead of reflection.

## Task Format

When adding new tasks, prefer this structure:

- `[ ] task`
- optional note or link to related file/feature

## Change Policy

Update this file frequently. Keep it compact — move details to feature docs.