# Monster Item Rewards

## Purpose

Add additional item rewards for victory in monster fights during travel.

## Domain References

- `docs/domain/combat.md`
- `docs/domain/items.md`

## Scope

- Applies only to victory in `MONSTER_FIGHT` activity actions.
- Gold reward remains part of the reward flow.
- Item rewards are defined per monster kind (`MonsterMold`).
- Item rewards may be absent for some monster kinds.

## Acceptance Criteria

- Monster-fight reward generation keeps existing gold calculation.
- Monster-fight reward generation may append additional item rewards configured for the defeated monster kind.
- Item reward configuration supports one record per independent drop roll.
- Each reward record contains: monster kind, item, chance percent, amount.
- Multiple records with the same item are allowed for the same monster kind.
- Before granting rewards to hero inventory, only stackable dropped items are grouped by item identity and amounts are summed.
- Non-stackable dropped items are not grouped and are granted as separate item instances.
- Reward DTO returned by monster-fight endpoints includes all dropped items.
- Finalization grants dropped items to the hero inventory using existing give-item flow.
- Initial runtime seed defines a starting reward table for existing monster kinds.
- Fight UI shows received reward items on the fight screen, including item image.
- Fight UI reward panel displays grouped rows only for stackable items; non-stackable items are displayed as separate rows.

## Architecture Contract

- Store monster item reward entries in DAL as a relation from `MonsterMold` to `Item` with `ChancePercent` and `Amount`.
- Do not introduce EF migrations. Update schema in `ApplicationContext` and rely on manual database recreation when needed.
- Load reward entries together with monster mold data needed for reward generation.
- Keep reward generation in `MonsterFightService` so the public fight contract stays unchanged.
- Reuse existing `MonsterFightReward -> GivenItem -> GiveItemsToHeroAsync` flow.

## Data Notes

- The technical model intentionally uses independent reward rows instead of quantity ranges.
- This supports guaranteed drops and extra rare copies without extra rule parsing.
- Duplicate drops are consolidated only for stackable items in the final reward projection before hero grant and UI display.

## Backend Steps

1. Add DAL/entity and mapping support for monster reward entries.
2. Extend monster mold loading to include reward entries and referenced items.
3. Update monster-fight reward generation to append rolled item rewards.
4. Seed initial reward entries for existing monster kinds.
5. Add focused tests for reward generation and reward persistence flow.

## Known Technical Debt

- `MonsterFightRewardTests` currently tests `AggregateStackableItems` via reflection into a private method of `MonsterFightService`. This is fragile and couples the test to implementation details. When refactoring the service, extract the aggregation logic into a proper testable unit and replace the reflection-based test.

## Frontend Steps

1. Reuse existing fight reward panel.
2. Render reward items as structured entries instead of plain HTML string.
3. Show item image, name, and amount for each received reward item.

## Rejected Path

- Quantity ranges per reward row were rejected for now.
- Independent rows are easier to balance, test, and reason about for MVP.
