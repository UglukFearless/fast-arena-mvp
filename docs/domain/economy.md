# Economy

## Inventory Visibility Rules (Current MVP)

- Dead owned hero inventory is available only in read-only mode.
- Inventory of a foreign hero is not available.

## Shop

- Heroes can buy and sell items at the shop.
- Purchases are paid with gold; selling items adds gold to the shop balance.
- A transaction bundles sell and buy selections into a single confirmed deal.
- The hero's gold balance changes by the net result of the transaction.
- Items that are not stackable (`CanBeFolded = false`) occupy individual cells and cannot be accumulated.
- Equipped items are excluded from the shop selling list.

## Change Policy

Update this file when inventory rules, shop behavior, or transaction logic change.
