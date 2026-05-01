namespace FastArena.Dal.Entities;

public class MonsterRewardEntryDal
{
    public Guid Id { get; set; }
    public Guid MonsterMoldId { get; set; }
    public MonsterMoldDal MonsterMold { get; set; }
    public Guid ItemId { get; set; }
    public ItemDal Item { get; set; }
    public int ChancePercent { get; set; }
    public int Amount { get; set; }
}
