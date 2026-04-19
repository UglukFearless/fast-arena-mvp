# Todo

## Purpose

This document tracks short-horizon tasks. It should stay compact and operational.

## Current Priorities

### Documentation

- [ ] Finalize the structure of project documentation.
- [x] Fill `docs/domain.md` with concrete business rules.
- [ ] Fill `docs/architecture.md` with codebase-specific examples.

### Next Goal: Hero Inventory

- [x] Rework item seed. All items seeded via runtime `ItemSeeder`; images served from backend `wwwroot/assets/items/`.
- [x] Inventory with items is integrated into `HeroInfo`.
- [x] Define and implement item acquisition path for heroes. Shop implemented: hero can buy and sell items; transactions confirmed atomically.

### Next Goal: Item Effects, Pockets, and Combat Usage

Implementation note: [`docs/features/item-effects.md`](features/item-effects.md)

- [x] Introduce item effect/parameter model and storage for consumable/support items (potions included).
- [x] Add hero equipment system with 3 pockets for support items.
- [x] Finalize domain rules for in-fight item usage and effect timing/stacking semantics.
- [x] Finalize architecture contract for unified effect hooks and effect-type handlers.
- [x] Implement item usage flow in fight: consume equipped item, apply effect, persist battle state updates.
- [x] Complete frontend fight integration for item usage (`USE_ITEM` action + pocket selection in combat).
- [x] Show active effect indicators in fight UI (including temporary icon URL from backend contract).
- [x] Synchronize domain docs with implemented item/effect combat behavior and keep planned markers only for truly pending mechanics.

## Task Format

When adding new tasks, prefer this structure:

- `[ ] task`
- optional owner
- optional note or link to related file/feature

## Change Policy

Update this file frequently. Remove completed items when they no longer matter, or move major completed work into changelog or roadmap notes.