namespace FastArena.Core.Domain.Heroes;

public class HeroEquippedSlot
{
    public Guid HeroId { get; set; }
    public EquipmentSlotType Slot { get; set; }
    public Guid? HeroItemCellId { get; set; }

    public Hero? Hero { get; set; }
    public HeroItemCell? HeroItemCell { get; set; }
}