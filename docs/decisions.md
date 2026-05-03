# Technical Decisions

## Purpose

Log of tactical technical decisions: what was chosen and why.

Entries are short (4–5 lines). Business rules stay in `docs/domain/`. Architecture structure stays in `docs/architecture.md`. This file records the rationale behind choices that affect multiple modules or that could plausibly be revisited.

## Decisions

### Effect Application Phase And Operation Type Are Runtime Concerns

- **Decision:** Application phase and operation type are not stored in effect definition storage. They are resolved by typed runtime handlers.
- **Why:** Effect definitions are data; logic belongs in code. Persisting operation type would couple the storage schema to runtime behavior and make handler evolution brittle.
- **Applies to:** Item effects, potions, future active abilities.
- **Status:** Active.

### Hero DAL Mapping Depth For Fight History

- **Decision:** When a Hero response needs nested fight history, use deep mapping (`MonsterFightProfile.Map(..., true)`). Default shallow mapping leaves `Monster.Portrait` null.
- **Why:** EF Core lazy loading is not used; includes must be explicit. Deep mapping is opt-in to avoid over-fetching in contexts that do not need full fight history.
- **Applies to:** HeroInfo endpoints, statistics responses.
- **Status:** Active.

## Format

Each entry follows this structure:

```
### Short Title (noun phrase)
- **Decision:** What was chosen.
- **Why:** Rationale (1–2 lines).
- **Applies to:** Affected modules or use cases.
- **Status:** active | pending | superseded.
```

### ActiveEffect State Lives In Fight State, Server-Authoritative

- **Decision:** `ActiveEffect` instances are serialized into fight session state on the server. Each round response returns the current active effect snapshot to the frontend. No separate client-side state mechanism.
- **Why:** The server is the single source of truth for fight state. Introducing a parallel client-side or session-cache mechanism would create sync risks. Fight state already travels with each round response.
- **Applies to:** Fight service, fight round DTO, frontend fight store.
- **Status:** Active.

### Effect Stacking Normalization Happens At Round Start

- **Decision:** Stacking normalization runs at the start of each round (Phase A), before any hook fires.
- **Why:** Ensures all effects are in a consistent merged state before initiative, ability calculation, and damage hooks evaluate them. Per-type stacking rules run in this phase.
- **Applies to:** Fight service hook dispatch, `IEffectHandler` stacking responsibility.
- **Status:** Active.

### Equipment Effects Use The Shared Effect Pipeline With Persistent Lifetime

- **Decision:** Weapon and shield combat modifiers are implemented as persistent effects inside the existing effect pipeline, not as a separate modifier system.
- **Why:** Reuses handler registry, hook dispatch, and fight state structure already in place. A parallel system would duplicate lifecycle and ordering logic for no gain.
- **Applies to:** `MonsterFightService`, `ActiveEffect`, `IEffectHandler`, fight round DTO, weapon and shield seed data.
- **Status:** Active.

### ActiveEffect Extended With LifetimeType And SourceType

### Equipment Effects Are Not Displayed In Fight UI

- **Decision:** Effects with `SourceType = Equipment` are filtered from the active effects list in the fight round response. Equipment influence is expressed through round outcome values, not through effect indicators.
- **Why:** Persistent equipment effects are always active and carry no per-round state worth surfacing to the player. Effect indicators are meaningful for consumables with limited duration. If a specific equipment item needs UI representation in the future, it will receive a dedicated `SourceType` value.
- **Applies to:** Fight round DTO mapping, frontend active effect rendering.
- **Status:** Active.

- **Decision:** `ActiveEffect` gains `LifetimeType` (`RoundBased` | `Persistent`) and `SourceType` (`Potion` | `Equipment` | `Skill`).
- **Why:** Decrement and cleanup logic must distinguish persistent equipment effects from expiring consumable effects. Source type enables correct initialization, ordering, and future UI attribution.
- **Applies to:** `ActiveEffect`, fight service decrement/cleanup logic, fight round DTO.
- **Status:** Active.

## Change Policy

Add an entry when a non-obvious technical choice is made that affects multiple modules or could be revisited.
Update status to `superseded` when a decision is reversed; do not delete the entry until the superseding decision is stable.
