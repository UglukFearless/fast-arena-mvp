using FastArena.Core.Domain.Heroes;

namespace FastArena.Dal.Entities;

public class ItemAllowedSlotDal
{
    public Guid ItemId { get; set; }
    public EquipmentSlotType Slot { get; set; }

    public ItemDal? Item { get; set; }
}