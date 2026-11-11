using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Domain.Items;

/// <summary>
/// Slot an item occupies, declared by the slot key. Rows repeat when an item needs several slots of one key:
/// a two-pocket amulet declares two POCKET rows.
/// </summary>
public class ItemSlot
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public required string SlotKey { get; set; }

    public Item? Item { get; set; }
}
