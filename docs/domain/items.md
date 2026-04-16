# Items

## Entity

- An object that exists in the game world and can be held by a hero.
- Has a name, description, base cost, and an image.
- Belongs to one of several types that define how it behaves.

## Item Types

- **Money** — currency used for purchases. Gold is the only money item.
- **Potion** — consumable used during combat. Restores HP or temporarily modifies combat characteristics.
- **Weapon** — equippable item that modifies attack damage or attack power.
- **Shield** — equippable item that provides a chance to block incoming attacks.
- **Armor** — equippable protective item. Reserved for future use.
- **Other** — loot and collectibles with no active effect. Sold for gold.

## Behavioral Flags

- `CanBeFolded` — stackable in a single inventory cell (e.g. gold, loot drops).
- `CanBeEquipped` — can be actively equipped or used by the hero (e.g. potions, weapons, shields).

## Combat Usage Status

- Implemented (MVP now):
	- Item taxonomy exists (`Money`, `Potion`, `Weapon`, `Shield`, `Armor`, `Other`).
	- Behavioral flags exist (`CanBeFolded`, `CanBeEquipped`).
- Planned (not fully implemented yet):
	- Distinct combat effects model for usable/equippable items.
	- Equipment influence on combat characteristics.
	- Equipment influence on outgoing and incoming damage.
	- Full in-fight item usage flow and effect persistence.

## Combat Item Subdomains

### Weapon (Planned, Not Implemented)

- Weapon can be one-handed or two-handed.
- Two weapons at the same time (one in each hand) are not allowed.
- If a one-handed weapon is equipped, the second hand may equip a shield.
- A two-handed weapon occupies both hands and excludes shield usage.

Weapon may modify outgoing damage by two independent mechanisms:

- Both mechanisms may coexist on the same weapon.
- Weapon modifiers are applied only after a strike is confirmed; they do not affect the fact of hit/miss.

1. Strike power modifier

- Changes strike power after successful attack resolution.
- Can be positive or negative.
- Example formula:
	- `FinalDamage = (StrikePower + WeaponStrikePowerDelta) * ZoneUnitDamage`

2. Flat damage per strike power unit

- Adds or subtracts a fixed HP amount for each strike power unit.
- Can be positive or negative.
- Example formula:
	- `FinalDamage = (StrikePower * ZoneUnitDamage) + (WeaponFlatPerPowerDelta * StrikePower)`

Near-term note:

- When both mechanisms are active, implementation should keep a deterministic and explicit calculation order.
- Negative final damage is not intended.

Long-term direction:

- Weapons may apply additional effects, including probabilistic effects (for example bleeding or poisoning).

### Shield (Planned, Not Implemented)

- Shield is evaluated only after it is confirmed that the participant receives a strike.
- Shield may fully block incoming damage with a configured probability.
- Block probability is represented in percent values, not in dice-face thresholds.
- Shield has a finite number of successful block uses in a fight.
- Each successful full block consumes one use.
- After all uses are consumed, the shield is considered broken for the current fight and provides no further protection.

### Armor (Planned, Not Implemented)

- Armor is equipped against the hit-zone map (for example: helmet -> head, greaves -> legs).
- Armor pieces can cover one or several zones (for example: cuirass may cover body zones but not neck/groin).
- The same zone cannot be covered by two armor pieces at once.
- If two armor pieces overlap by at least one zone, they conflict and cannot be equipped together.
- Near-term implementation note: avoid overlap-conflict-heavy item definitions where possible.

Armor may mitigate damage by two mechanisms:

1. Strike power absorption with durability

- Armor absorbs part of incoming strike power before HP damage is calculated.
- Example: incoming strike power is 4, armor durability is 2 -> armor absorbs 2 power, remaining strike power is 2.
- Absorbed power reduces armor durability by the same amount.
- When durability reaches 0, that armor no longer protects.

2. Flat damage absorption per strike power unit

- Armor reduces zone unit damage by a fixed amount for each strike power unit.
- Example: strike power is 4, zone unit damage is 10, armor absorbs 2 per power -> `4 * (10 - 2) = 32` instead of 40.
- Zone unit damage per 1 strike power cannot drop below 1.

### Usable Items And Pockets (Planned, Not Implemented)

Planned labels in this section are intentionally kept as temporary navigation markers.

#### Pockets

- Before a fight, the hero places usable items into pocket slots.
- Pockets are the only inventory slots accessible during combat and travel.
- Current pocket count: 3.
- If all pockets are occupied, placing another usable item into pockets is denied.

#### Slot Compatibility

- Equippable items have predefined allowed equipment slots.
- Potions are compatible with pocket slots.
- Other equippable categories may have a single deterministic slot set, but use the same compatibility concept.

#### Using An Item In Combat

- Using an item is a distinct round action, alternative to attack.
- The item is consumed on use and removed from the pocket.
- In a round where the hero uses an item, the hero applies the item effect but does not attack:
	- if the hero rolls higher in the initiative phase, the higher roll is ignored for strike purposes,
	- if the opponent rolls higher, the incoming strike is resolved by normal rules.
- Monster item usage mechanics are a long-term direction and are not planned in the near term.

#### Effect Model

Combat effects have three properties:

- **Application time** — when in the round lifecycle the effect is applied.
- **Duration** — number of rounds the effect is active. The round of use counts as round 1.
- **Target** — what characteristic or mechanic the effect modifies.

Application time and duration are different dimensions:

- Application time defines the phase where an effect can influence combat state.
- Duration defines how long that effect remains active across rounds.

#### Effect Types (Generalized)

Combat effects are defined by domain type plus parameters from data storage.

1. Resource modification effects

- Change resource values such as HP.
- Typical timing: immediate, before initiative roll of the same round.
- Typical parameters: target resource, amount, clamp rules.

2. Characteristic override effects

- Temporarily replace a calculated characteristic with an override value.
- Current known case: Ability override to hero maximum (`floor(MaxHP / 10)`) regardless of current HP.
- Typical parameters: characteristic name, override rule/value, duration.

3. Characteristic modifier effects

- Additive or multiplicative change to a calculated combat value.
- Current known case: strike power bonus on successful attack.
- Typical parameters: affected value, operation type, magnitude, duration, trigger conditions.

4. Triggered probability effects (future)

- Activate on specific combat events with configured probability.
- Typical parameters: trigger event, probability, payload effect.

5. Meta-economy effects (future)

- Affect non-damage combat outputs (for example reward quality/amount).
- Typical parameters: affected reward channel, operation type, magnitude, duration.

#### Effect Stacking

- Multiple active effects of the same type are allowed.
- Repeated usage of the same effect type is resolved by that effect type's stacking rule.
- Repeated usage of the same effect type always produces one aggregated active effect for that type.
- If effects are merged, merged parameters (for example magnitude and remaining rounds) are determined by the same effect-type rule.
- The round of usage still counts as round 1 for the resulting active effect lifecycle.

#### Future Effect Categories

- Passive trigger effects (no explicit use action):
	- Example: charm that passively reduces chance of a lethal blow.
	- Example: charm that increases loot reward on victory.
- These require a passive slot or always-on pocket mechanic to be designed separately.

## Change Policy

Update this file when item types, flags, or item entity rules change.
