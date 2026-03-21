
namespace FastArena.Core.Domain.Monsters;

public class MonsterMold
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int RankLevel { get; set; } = 1;
    public int HealthPerLevel { get; set; } = 10;
    public int BaseHealth { get; set; } = 100;
    public required MonsterSex Sex { get; set; } = MonsterSex.NONE;
    public Guid PortraitId { get; set; }
    public Portrait? Portrait { get; set; }
}
