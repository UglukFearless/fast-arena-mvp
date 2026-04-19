using FastArena.Core.Domain.Heroes;

namespace FastArena.Dal.Entities;

public class HeroEquippedSlotDal
{
    public Guid HeroId { get; set; }
    public EquipmentSlotType Slot { get; set; }
    public Guid? HeroItemCellId { get; set; }

    public HeroDal? Hero { get; set; }
    public HeroItemCellDal? HeroItemCell { get; set; }
}