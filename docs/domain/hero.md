# Hero

## Entity

- A player-controlled character.
- Has a portrait, characteristics, and progression state.
- Participates in fights and accumulates results.

## Hero Combat Characteristics

- The hero currently has Health Points (HP).
- The hero also has Ability.
- At the current MVP stage, Ability is derived from current HP by integer division.
- Formula: `Ability = floor(currentHP / 10)`.
- This means an exhausted (damaged) hero becomes progressively less effective in combat.

## Planned Evolution Of Characteristics

- Ability derivation may evolve beyond the current HP-based formula.
- Additional factors beyond current health may influence Ability.
- Passive and active skills are planned for future iterations.

## Hero Ownership Per User

- At MVP stage, one user can have an unlimited number of heroes.

## Change Policy

Update this file when hero characteristics, progression rules, or ownership rules change.
