using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Domain.Items;

public class ItemAllowedSlot
{
    public Guid ItemId { get; set; }
    public EquipmentSlotType Slot { get; set; }

    public Item? Item { get; set; }
}