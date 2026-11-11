namespace FastArena.Dal.Entities;

public class HeroSlotDal
{
    public Guid Id { get; set; }
    public Guid HeroId { get; set; }
    public required string SlotKey { get; set; }
    public Guid? HeroItemCellId { get; set; }

    public HeroDal? Hero { get; set; }
    public HeroItemCellDal? HeroItemCell { get; set; }
}
