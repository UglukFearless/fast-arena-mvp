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
- `OnRoundEnd` — apply end-of-round hooks and decrement remaining rounds (Phase G).
- Expired effects are removed at the beginning of the next round (`OnRoundStart` normalization step), so an effect that reached `0` this round is still visible in the produced state snapshot.

### Handler Registry

- Each `EffectType` maps to one `IEffectHandler` implementation.
- Handlers are stateless; all state lives in the `ActiveEffect` instance.
- The handler for a type is responsible for its own stacking rule when a new activation arrives.

## Implementation Phases

### Phase 1 — Persisted Definitions And Seed Data ✅

- Effect definition schema persisted in DAL; fields include Type, DurationRounds, Magnitude, MinValue, MaxValue, ChancePercent, ConditionType, TargetType, Priority, NextEffectDefinitionId.
- Potion seed baseline: heal 60 (100% chance), ability override to max for 3 rounds, strike power bonus +2 for 3 rounds.

### Phase 2 — Domain Rules Finalization ✅

- ✅ In-fight item usage rules documented: timing, passive mode, draw condition when passive and opponent does not hit (see `docs/domain/combat.md` Phases A–G and `docs/domain/items.md` Usable Items section).
- ✅ Stacking semantics per effect type finalized (see `docs/domain/effects.md`).
- ✅ Outcome: `docs/domain/items.md` and `docs/domain/combat.md` and `docs/domain/effects.md` reflect complete rules with no remaining ambiguity.

### Phase 3 — Architecture Contract ✅

- ✅ Define `ActiveEffect` in `FastArena.Core/Domain/Effects/ActiveEffect.cs`.
- ✅ Align handlers to operate directly on fight state values, avoiding ad-hoc context objects.
- ✅ Define `IEffectHandler` interface in `FastArena.Core/Interfaces/Effects/IEffectHandler.cs` with hooks: `OnRoundStart`, `OnStrikeClaimed`, `OnPowerModifiers`, `OnRoundEnd`, `Stack()`.
- ✅ Define `EffectHandlerRegistry` in `FastArena.Core/Services/Effects/EffectHandlerRegistry.cs` (maps `EffectType` → `IEffectHandler`).

### Phase 4 — Fight Service Wiring ✅

- ✅ Instantiate `ActiveEffect` when hero chooses item use at Phase A.
- ✅ Dispatch hooks through registry at the correct fight phases.
- ✅ Remove pocket item on use; update inventory.
- ✅ Decrement effect durations at round end and cleanup expired effects at the start of the next round.

### Phase 6 — Backend Test Coverage ✅

- ✅ Unit tests grouped under `backend/src/FastArena.Core.Tests/Unit/`:
	- `Effects/` — handlers and handler registry behavior.
	- `Services/MonsterFight/` — lifecycle semantics inside fight service helpers.
- ✅ Service-level integration scenario grouped under `backend/src/FastArena.Core.Tests/Integration/Services/MonsterFight/`.
- ✅ Current suite validates item-use state transition, pocket consumption, effect activation, and lifecycle invariants.

### Phase 5 — Frontend Integration ⏳

- Extend fight round DTO with active effect state (type, remaining rounds) visible to UI.
- Extend active effect DTO with a temporary `imageUrl` field for frontend rendering, resolved by backend from the current effect source.
- Update frontend to show used item and active effect indicators.
- Current icon strategy is temporary: active effect indicators use item-source icons.
- Future rework required: move to dedicated effect icons, or derive icon resolution from effect source/type through an explicit resolver policy.

## Rejected Paths

- **Store operation type in effect definition storage:** Would couple the storage schema to runtime logic. Rejected because handlers need full control over how an effect type is applied. See `docs/decisions.md` → "Effect Application Phase And Operation Type Are Runtime Concerns".
- **Persist `ActiveEffect` as a dedicated DAL table:** Adds migration complexity for MVP with little benefit. Preferred alternative: serialize into fight session state or reconstruct from fight event log.

## Change Policy

Update phases as implementation progresses. Move resolved open questions to `docs/decisions.md` once a path is chosen.
