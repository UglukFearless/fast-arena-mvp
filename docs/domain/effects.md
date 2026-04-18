# Effects

## Purpose

This page describes the effect system: how effects are defined, applied, and stacked.
Effects are a shared mechanic used by items, and in the future by hero skills and other sources.

## Effect Model

An effect has three core properties:

- **Application time** — when in the round lifecycle the effect is applied.
- **Duration** — number of rounds the effect is active. The round of activation counts as round 1.
- **Target** — what characteristic or mechanic the effect modifies.

Application time and duration are independent dimensions:

- Application time defines the phase where an effect can influence combat state.
- Duration defines how long that effect remains active across rounds.

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
- Stacking only applies when the existing effect of the same type is still active. If the previous effect has already expired, the new activation starts fresh without merging.
- Same-type effects always collapse into one aggregated active effect; two parallel effects of the same type do not coexist.

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

## Future Effect Categories

- Passive trigger effects (no explicit use action):
	- Example: charm that passively reduces chance of a lethal blow.
	- Example: charm that increases loot reward on victory.
- These require a passive slot or always-on pocket mechanic to be designed separately.
- Hero skills that apply effects will follow the same type/stacking model.

## Change Policy

Update this file when new effect types, stacking rules, or effect sources are introduced.
