# Feature: Weapon And Shield Usage In Combat

## Goal

Enable weapon and shield influence in combat through the shared effect pipeline, including persistent equipment effects during a fight.

## Domain References

- `docs/domain/items.md`
- `docs/domain/combat.md`
- `docs/domain/effects.md`

## Scope

- Weapon damage contribution in combat phases where strike power and damage are resolved.
- Shield full-block behavior with limited successful blocks per fight.
- Equipment effects are active while the item is equipped.

## Acceptance Criteria

1. Equipped weapon modifies outgoing strike power and/or final damage according to item effect definition.
2. Equipped shield can fully block incoming strike by configured chance and consumes limited successful blocks.
3. Equipment effects and consumable effects can coexist in one fight without semantic conflicts.
4. Fight response reflects effective round outcome after equipment and consumable effects are applied.

## Architecture Notes

- Reuse the existing effect runtime flow for equipment and consumables.
- Extend the active effect runtime model with:
	- `LifetimeType` — `RoundBased` or `Persistent`.
	- `SourceType` — `Potion`, `Equipment`, `Skill`, or future categories.
- Persistent effects are initialized at fight start from equipped items and are not subject to round countdown or stack merge.
- Round-based effects continue using existing per-type stacking rules.
- Keep effect-type behavior encapsulated by effect handlers.
- Keep fight service responsible for phase progression and deterministic execution order.

## Required Refactors

### Remove ConditionType

- Remove `ConditionType` field from `EffectDefinition` and `ActiveEffect`.
- Remove the `ConditionType` enum.
- Remove `ConditionType` from seed data.
- Effect applicability is owned by the typed handler; no generic condition field is needed.

### Add LifetimeType And SourceType To ActiveEffect

- Add `LifetimeType` (`RoundBased` | `Persistent`) to `ActiveEffect`.
- Add `SourceType` (`Potion` | `Equipment` | `Skill`) to `ActiveEffect`.

## Implementation Checklist

### Backend — Done

- [x] Remove `ConditionType` from `EffectDefinition`, `ActiveEffect`, enum, and seed data.
- [x] Add `LifetimeType` and `SourceType` to `ActiveEffect`.
- [x] Add new equipment effect types: `STRIKE_POWER_BONUS`, `UNIT_DAMAGE_DELTA`, `INCOMING_STRIKE_FULL_BLOCK`.
- [x] Initialize persistent equipment effects from equipped weapon and shield at fight start.
- [x] Wire `UNIT_DAMAGE_DELTA` into Phase E (before damage commit) of fight lifecycle.
- [x] Wire `INCOMING_STRIKE_FULL_BLOCK` into Phase C (incoming strike confirmed) of fight lifecycle.
- [x] Filter `SourceType = Equipment` effects from fight round DTO response.
- [x] Bind weapon and shield seed items to effect definitions.
- [x] Add backend tests for weapon and shield effect application and equip/unequip behavior.
- [x] Expand shop catalog and purchase validation to include `WEAPON` and `SHIELD`.
- [x] Enable equipping/unequipping weapons and shields via `HeroEquipmentService` with two-handed conflict checks.

### Frontend — Done

- [x] Allow equipping/unequipping weapons and shields from the hero equipment UI.
- [x] Show shield block result in fight round UI.
- [x] Add a dedicated equipment panel component in the fight window and bind it to hero equipped slots.
- [x] Extract active effects panel as a dedicated component in the fight window.
- [x] Move pocket item source of truth to hero equipped slots; remove pocket items from fight state DTO.

## Rejected Path

- Separate weapon/shield combat modifier pipeline outside the effect system.
- Rejected because it duplicates lifecycle and ordering logic already present in the effect pipeline.

## Change Policy

Update this file as implementation decisions are finalized and phases progress.
