# Effects

## Purpose

This page describes the effect system: how effects are defined, applied, and stacked.
Effects are a shared mechanic used by items, and in the future by hero skills and other sources.

## Effect Model

An effect has the following core properties:

- **Duration** — for round-based effects, number of rounds the effect is active. The round of activation counts as round 1.
- **Lifetime** — whether the effect expires by round count or persists for the entire fight.
- **Source** — the category of the source that produced the effect (for example: consumable item, equipped item, skill).
- **Target** — what characteristic or mechanic the effect modifies.

Lifetime and duration are distinct concepts:

- Round-based effects use duration to count down remaining rounds.
- Persistent effects last for the entire fight regardless of round count.

## Effect Types

Effects are defined by domain type plus configurable parameters.

### 1. Resource Modification

- Changes resource values such as HP.
- Typical timing: immediate, before initiative roll of the same round.
- Typical parameters: target resource, amount, clamp rules.

### 2. Characteristic Override

- Temporarily replaces a calculated characteristic with an override value.
- Current known case: Ability override to hero maximum (`floor(MaxHP / 10)`) regardless of current HP.
- Typical parameters: characteristic name, override rule/value, duration.

### 3. Characteristic Modifier

- Additive or multiplicative change to a calculated combat value.
- Current known case: strike power bonus on successful attack.
- Typical parameters: affected value, operation type, magnitude, duration, trigger conditions.

### 4. Triggered Probability Effects (Future)

- Activate on specific combat events with configured probability.
- Typical parameters: trigger event, probability, payload effect.

### 5. Meta-Economy Effects (Future)

- Affect non-damage combat outputs (for example reward quality/amount).
- Typical parameters: affected reward channel, operation type, magnitude, duration.

## Effect Stacking

### General Rules

- Stacking is evaluated at the start of each round, before any hook fires.
- Persistent effects (from equipment) do not participate in the stack merge mechanism. Each equipped item contributes its effect independently; their contributions are aggregated in the relevant phase.
- Round-based effects from consumables follow per-type stacking rules below. Stacking only applies when the existing effect of the same type is still active. If the previous effect has already expired, the new activation starts fresh without merging.
- Same-type round-based effects always collapse into one aggregated active effect; two parallel round-based effects of the same type do not coexist.

### Per-Type Stacking Rules

#### Healing Effect (Resource Modification)

- Restores HP at round start, before initiative.
- Stacking formula: new magnitude = sum of magnitudes ÷ average duration (standard mathematical rounding). Duration of the merged effect = the averaged duration.
- Because a one-round heal fires and expires within the same round, two consecutive one-round heals cannot be active at the same time and do not stack; each starts fresh.

#### Ability Maximization Effect (Characteristic Override)

- Overrides the hero's Ability to the maximum healthy value regardless of current HP.
- Stacking rule: remaining durations are summed. The merged effect continues with the combined round count.
- Example: first effect has 2 rounds remaining; new 3-round dose is used → merged effect has 5 rounds remaining.

#### Strike Power Boost Effect (Characteristic Modifier)

- Adds a flat bonus to the hero's strike power on a confirmed hit.
- Stacking formula: new magnitude = sum of magnitudes ÷ average duration, rounded up (ceiling). Rounding up is intentional — using the same effect twice carries risk, and a small bonus rewards it.
- Example: two boosts where formula yields 2.2 → result is 3.

## Equipment Effects

- Effects from equipped items (for example weapon or shield) are persistent: they are active for the entire fight from the moment combat starts.
- Equipment effects follow the same phase model as round-based effects.
- Equipment effects do not merge with each other or with round-based effects of the same type. Each contributes independently in the relevant phase.
- Equipment slot rules (one item per slot) naturally prevent duplicate effects from the same slot.

## Future Effect Categories

- Passive trigger effects (no explicit use action):
	- Example: charm that passively reduces chance of a lethal blow.
	- Example: charm that increases loot reward on victory.
- These require a passive slot or always-on pocket mechanic to be designed separately.
- Hero skills that apply effects will follow the same type/stacking model.

## Change Policy

Update this file when new effect types, stacking rules, or effect sources are introduced.
