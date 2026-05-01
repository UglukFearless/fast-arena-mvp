using FastArena.Core.Domain.Items;

namespace FastArena.Core.Domain.Monsters;

public class MonsterRewardEntry
{
    public Guid MonsterMoldId { get; set; }
    public Guid ItemId { get; set; }
    public required Item Item { get; set; }
    public int ChancePercent { get; set; }
    public int Amount { get; set; }
}
