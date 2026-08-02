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

- Stackable items can be accumulated in one inventory cell (for example gold or simple loot).
- Equippable items can be actively equipped or used by the hero (for example potions, weapons, shields).

## Combat Usage Status

- Implemented (MVP now):
	- Item taxonomy exists (money, potion, weapon, shield, armor, other).
	- Behavioral flags for stacking and equipping exist.
	- Hero pocket system (3 slots) for usable combat items.
	- In-fight item usage action with pocket consumption.
	- Runtime active-effect model with stacking and duration lifecycle.
- In progress:
	- Weapon combat modifiers (permanent while equipped).
	- Shield full-block protection (permanent while equipped, limited uses per fight).
- Planned (not yet started):
	- Armor influence on incoming damage.
	- Extended equipment influence on combat characteristics.

## Combat Item Subdomains

### Weapon (In Progress)

- Weapon can be one-handed or two-handed.
- Two weapons at the same time (one in each hand) are not allowed.
- If a one-handed weapon is equipped, the second hand may equip a shield.
- A two-handed weapon occupies both hands and excludes shield usage.

Weapon may modify outgoing damage by two independent mechanisms:

- Weapon combat modifiers are active for the entire fight while the weapon is equipped. They do not expire per round and are not consumed by use.
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

### Shield (In Progress)

- Shield protection is active for the entire fight while the shield is equipped. Block uses are consumed during the fight but do not expire per round; the shield protects until all uses are spent.
- Shield is evaluated only after it is confirmed that the participant receives a strike.
- Shield may fully block incoming damage with a configured probability.
- Block probability is represented in percent values, not in dice-face thresholds.
- Shield has a finite number of successful block uses in a fight.
- Each successful full block consumes one use.
- After all uses are consumed, the shield is considered broken for the current fight and provides no further protection.

### Armor (In Progress)

- Armor is equipped against the hit-zone map (for example: helmet -> head, greaves -> legs).
- Armor pieces can cover one or several zones (for example: cuirass may cover body zones but not neck/groin).
- Two armor pieces cannot be equipped simultaneously if their protected zones overlap.
- Armor protection is active for the entire fight while the armor is equipped.

Armor mitigates incoming damage as follows:

- Armor has a protection value that applies to all zones it protects.
- When a strike is confirmed for a protected zone, armor subtracts its protection value from the strike power.
- Armor first reduces the incoming strike power, then damage is calculated from the remaining strike power.
- If the strike power is lower than armor protection, armor absorbs the entire strike and the target receives no damage.
- Protection value is consumed sequentially across all incoming strikes in a round.
- Once protection value is exhausted (reaches 0), the armor breaks and provides no further protection for the rest of the fight.
- Example: armor protection is 4; hero receives strikes of 2 and 3 power in the same round. The first strike is fully absorbed (2 < 4, armor left with 2). The second strike: armor absorbs 2, hero receives damage 1 (3 - 2). Armor is now broken and provides no protection for further strikes that round.

### Usable Items And Pockets (Implemented MVP)

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

Effects applied by items follow the shared effect model. See [`docs/domain/effects.md`](effects.md).

## Change Policy

Update this file when item types, flags, or item entity rules change.
