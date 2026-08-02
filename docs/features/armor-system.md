# Feature: Armor System

## Goal

Enable armor pieces to protect hero zones during combat, extending the equipment system with zone-based coverage, conflict rules, and per-fight protection durability.

## Domain References

- `docs/domain/items.md` — armor type, zone coverage, protection mechanics, conflict rules.
- `docs/domain/combat.md` — Phase 4 damage calculation, Phase E power modifier ordering.

## Scope

- Hero can buy armor pieces from the shop.
- Hero can equip armor pieces; equipping is blocked if the new piece overlaps zones with an already equipped piece.
- Equipped armor reduces incoming strike power for covered zones during a fight.
- Armor protection is exhausted sequentially across strikes within a fight; once exhausted, the armor piece provides no further protection until the next fight.

## Acceptance Criteria

1. Armor items are available in the shop and can be purchased by the hero.
2. Hero can equip an armor piece if no currently equipped piece covers overlapping zones.
3. Equipping a conflicting armor piece is rejected with a clear reason.
4. During a fight, an incoming strike against a covered zone reduces its effective strike power by the armor's protection value before damage is calculated.
5. If armor protection is greater than or equal to incoming strike power, no HP damage is applied.
6. Armor protection is consumed across strikes in order; once at zero, the armor piece is inactive for the rest of the fight.
7. Fight round response reflects armor protection state (active or broken).
8. After the fight, armor remains equipped and protection is restored for the next fight.

## Architecture Notes

- Armor protection is a fight-scoped runtime state, analogous to shield uses remaining.
- Armor protection evaluation is placed at Phase E of the fight lifecycle (power-level damage modifiers), before zone unit damage is applied.
- Zone coverage and conflict validation belongs to `HeroEquipmentService`.
- Hit-zone map is shared across all participants (current MVP limitation).

## Implementation Checklist

### Phase 1 — Domain And Data Model

- [ ] Define armor item structure: protected zones list and protection value.
- [ ] Implement zone overlap conflict check in equipment service.
- [ ] Seed initial armor item catalog with zone coverage and protection values.

### Phase 2 — Shop And Equip Flow

- [ ] Extend shop to include armor items.
- [ ] Enable equip/unequip armor via equipment service with conflict validation.
- [ ] Add backend tests for equip conflict rules.

### Phase 3 — Combat Integration

- [ ] Initialize armor runtime state (remaining protection) at fight start from equipped armor.
- [ ] Wire armor protection absorption into Phase E of fight lifecycle.
- [ ] Extend fight round DTO to include armor state (protection remaining per piece).
- [ ] Add backend tests for armor protection consumption and breakage.

### Phase 4 — Frontend

- [ ] Show equipped armor pieces in hero equipment UI.
- [ ] Show armor state (active / broken) in fight window.

## Out Of Scope

- Armor durability persisting after fight (armor restores fully after each fight).
- Armor repair mechanics.
- Zone-specific hit zone maps per creature type (long-term direction).
- Flat damage absorption per strike power unit (not part of MVP armor model).

## Rejected Paths

- Two independent armor damage mitigation mechanisms (strike power absorption + flat damage per power unit): replaced by single protection-value model for clarity and MVP scope.

## Change Policy

Update this file as implementation decisions are finalized and phases progress.
