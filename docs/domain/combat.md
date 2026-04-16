# Combat

## Purpose

This page describes the currently implemented round-based combat model in domain terms.

## Participants

- Combat is a duel between two participants (hero and opponent).
- Each participant has at least two combat-relevant characteristics:
	- HP
	- Ability

## Core Combat Principle

- Combat consists of consecutive rounds of the same structure.
- Combat continues until one participant dies.
- Death condition: HP is less than or equal to 0.

## Characteristics And Derived Advantage

### Ability (Current Formula)

- In current implementation, Ability is derived by integer division of current HP by 10.
- Formula: `Ability = floor(currentHP / 10)`.
- Ability is recalculated each round based on current health, meaning a damaged participant has reduced advantage.

### Relative Advantage

- Advantage is computed from Ability difference between attacker and defender.
- Formula base: `floor((A1 - A2) / 3)`.
- The result keeps its sign and is clamped to the range `[-3, +3]`.
- Positive value means A1 has advantage over A2; negative value means A2 has advantage.

Examples:

- Ability 10 vs 7 gives advantage `+1`.
- Ability 13 vs 6 gives advantage `+2`.

## Round Phases

### Phase 1: Initiative Roll

- Both participants roll a six-sided die (`d6`).
- Roll difference defines who claims the strike and with what base strike power.
- Example: hero rolls 5, opponent rolls 3 -> hero claims strike with base power 2.

### Phase 2: Advantage Mitigation

- Defender advantage is subtracted from the base strike power.
- If resulting strike power is less than 1, the round is treated as a draw (no strike).

### Phase 3: Hit Zone Roll

- If strike power is 1 or more, hit zone is determined.
- Current MVP uses one legacy damage map.
- Legacy hit zone flow is two-step:
	- First roll identifies broad zone set.
	- For body result, second roll identifies concrete sub-zone.

### Hit Zone Maps (Future)

- Long-term direction: introduce different hit zone maps for different creature types.
- Current MVP limitation: one shared legacy map is used for all participants.

### Phase 4: Damage Calculation

- Each hit zone has a damage price per 1 strike power.
- Total damage formula:
	- `Damage = ZoneUnitDamage * StrikePower`
- Example: head has unit damage 20 HP.
	- Strike power 1 -> 20 HP damage.
	- Strike power 2 -> 40 HP damage.

### Phase 5: State Update And End Check

- Calculated damage is subtracted from target HP.
- System checks death condition for both participants.
- If nobody died, next round starts.

## Combat Items And Equipment

- Combat design includes equipment and usable items that may affect:
	- combat characteristics,
	- outgoing damage,
	- incoming damage.
- Weapon damage modifiers are applied after a strike is confirmed and do not affect hit eligibility.
- Source of truth for item categories and behavior flags: [Items](items.md).
- Current implementation status for combat item mechanics is tracked in [Items](items.md).

## Round Lifecycle For Effect Processing

This section defines business phases where active effects may influence a round.
The phase names are domain-oriented and describe gameplay semantics.
Phases or rules marked **[Planned]** are intentionally kept as navigation markers for upcoming behavior.

### Phase A: Round Start

- Available hero actions while both participants are alive:
	- attack,
	- use combat item **[Planned]**.
- Once HP reaches 0, combat round action is no longer available and combat goes to finalization.
- Active effects are normalized at round start according to their stacking rules. **[Planned]**
- If item usage is chosen:
	- item effects start influencing the round before initiative,
	- consumed item is removed from pocket,
	- hero enters passive mode for this round (hero does not attack). **[Planned]**

### Phase B: Strike Claim

- Resolve initiative rolls (`d6` each) and base strike power by difference.
- Determine effective Ability for both sides: use override value if an active effect requires it, otherwise `floor(currentHP / 10)`. **[Planned]**
- Apply Ability-derived advantage mitigation.
- If hero is in passive mode this round due to item usage: hero's roll is used only for defense; a higher hero roll does not result in a strike.
- If resulting strike power is less than 1 (or hero is passive and opponent does not hit), the round ends as a draw.

### Phase C: Confirmed Incoming Strike (Defense Window) **[Planned]**

- Reached only when strike power >= 1.
- Evaluate shield full-block probability.
- If shield fully blocks:
	- consume one successful shield use,
	- skip phases D–F,
	- continue to Phase G.

### Phase D: Hit Zone Resolution

- Determine hit zone using current legacy two-step map.

### Phase E: Power-Level Damage Modifiers **[Planned]**

- Apply strike-power-level modifiers:
	- active strike power bonus from usable items (attacker side),
	- weapon strike power modifier (attacker side),
	- armor strike power absorption with durability (defender side, zone-specific).
- If resulting strike power drops below 1, HP damage is not applied.

### Phase F: Unit-Damage Modifiers And Final Damage **[Planned]**

- Determine base zone unit damage.
- Apply per-power unit-damage modifiers:
	- armor flat absorption per strike power unit (zone unit damage floor is 1),
	- weapon flat bonus/penalty per strike power unit.
- Final damage is clamped to non-negative.

### Phase G: Commit And Round End

- Apply final damage to target HP.
- Recalculate base Ability for both participants from updated HP.
- Apply post-hit and end-of-round effects that are tied to this phase. **[Planned]**
- Produce final round projection for UI from effective values, including active overrides. **[Planned]**
- Check death condition and decide whether combat continues.

### Fight Finalization

- If one participant is dead, the fight enters finalization.
- Finalization resolves fight outcome, rewards/penalties, and history-relevant result.

## Fight Result

- A fight always produces a result that is meaningful for progression and history.
- Fight results are used for player feedback, statistics, and hero history.

## Fight Rewards And Penalties

- Rewards: experience and gold.
- Gold is used for economy flows (shop transactions and future item usage).
- Current hard penalty: permanent death.
- Additional debuffs may be added later.

## Change Policy

Update this file when combat mechanics, monsters, or fight outcome rules change.
