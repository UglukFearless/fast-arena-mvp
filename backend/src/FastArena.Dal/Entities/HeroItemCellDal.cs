namespace FastArena.Dal.Entities;

public class HeroItemCellDal
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid HeroId { get; set; }
    public int Amount { get; set; }

    public ItemDal? Item { get; set; }
    public HeroDal? Hero { get; set; }
    public ICollection<HeroEquippedSlotDal> EquippedSlots { get; set; } = new List<HeroEquippedSlotDal>();
}
