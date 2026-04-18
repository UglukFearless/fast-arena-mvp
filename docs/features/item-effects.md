# Feature: Item Effects Runtime Pipeline

## Goal

Enable heroes to use potions and other consumable items during combat, applying typed effects to combat state with correct phase ordering, duration tracking, and stacking rules.

## Domain References

- Item types, pockets, effect model, and stacking rules: [`docs/domain/items.md`](../domain/items.md)
- Fight round phases and effect application hooks: [`docs/domain/combat.md`](../domain/combat.md)

## Acceptance Criteria

1. A hero can equip potions into pocket slots before a fight.
2. During a fight round, the hero can choose to use an item instead of attacking.
3. The correct effect (heal, ability override, strike power bonus) is applied at the correct round phase.
4. Duration tracking decrements remaining rounds after each applicable round.
5. Same-type repeated usage is merged according to that effect type's stacking rule.
6. Consumed item is removed from pocket on use.
7. Fight round result returned to the frontend reflects active effect outcomes.

## Architecture Contract

### Modules Involved

- `FastArena.Core` — `ActiveEffect` model, `IEffectHandler` interface, hook dispatch in fight service.
- `FastArena.Dal` — Serialized or session-level storage for active effects between rounds.
- `FastArena.WebApi` — Fight round DTO extended with active effect state.
- `FastArena.WebHost` — No changes expected.

### ActiveEffect Model (Runtime)

Fields:

- `DefinitionId` — reference to the persisted effect definition.
- `EffectType` — copied from definition at activation time.
- `RemainingRounds` — decremented after each applicable round.
- `Parameters` — copied magnitude, min/max, chance, condition type, target type from definition.
- `StackCount` — for merged same-type effects; resolved by the handler's stacking rule.

### Fight Lifecycle Hooks

Phase names mirror `docs/domain/combat.md` domain phases:

- `OnRoundStart` — normalize active effects; apply resource modification effects (e.g. heal potion).
- `OnStrikeClaimed` — apply ability override effects (Phase B).
- `OnPowerModifiers` — apply strike power bonus effects (Phase E).
- `OnUnitDamageModifiers` — apply unit-damage modifier effects (Phase F).
- `OnRoundEnd` — decrement remaining rounds; remove expired effects (Phase G).

### Handler Registry

- Each `EffectType` maps to one `IEffectHandler` implementation.
- Handlers are stateless; all state lives in the `ActiveEffect` instance.
- The handler for a type is responsible for its own stacking rule when a new activation arrives.

## Implementation Phases

### Phase 1 — Persisted Definitions And Seed Data ✅

- Effect definition schema persisted in DAL; fields include Type, DurationRounds, Magnitude, MinValue, MaxValue, ChancePercent, ConditionType, TargetType, Priority, NextEffectDefinitionId.
- Potion seed baseline: heal 60 (100% chance), ability override to max for 3 rounds, strike power bonus +2 for 3 rounds.

### Phase 2 — Domain Rules Finalization ⏳

- Finalize in-fight item usage rules (timing, passive mode, draw condition when passive and opponent does not hit).
- Finalize stacking semantics per effect type.
- Outcome: `docs/domain/items.md` and `docs/domain/combat.md` reflect complete rules with no remaining ambiguity.

### Phase 3 — Architecture Contract ⏳

- Define `ActiveEffect` in `FastArena.Core/Models/`.
- Define `IEffectHandler` interface and hook method signatures in `FastArena.Core/Interfaces/`.
- Define `EffectHandlerRegistry` (maps `EffectType` → `IEffectHandler`).
- No fight service wiring yet; just interfaces and data models.

### Phase 4 — Fight Service Wiring ⏳

- Instantiate `ActiveEffect` when hero chooses item use at Phase A.
- Dispatch hooks through registry at the correct fight phases.
- Remove pocket item on use; update inventory.
- Decrement and expire effects at round end.

### Phase 5 — Frontend Integration ⏳

- Extend fight round DTO with active effect state (type, remaining rounds) visible to UI.
- Update frontend to show used item and active effect indicators.
- Active effect indicators use the source item's icon (potion icon for MVP). Per-effect-type icons are a future enhancement.

## Rejected Paths

- **Store operation type in effect definition storage:** Would couple the storage schema to runtime logic. Rejected because handlers need full control over how an effect type is applied. See `docs/decisions.md` → "Effect Application Phase And Operation Type Are Runtime Concerns".
- **Persist `ActiveEffect` as a dedicated DAL table:** Adds migration complexity for MVP with little benefit. Preferred alternative: serialize into fight session state or reconstruct from fight event log.

## Change Policy

Update phases as implementation progresses. Move resolved open questions to `docs/decisions.md` once a path is chosen.
