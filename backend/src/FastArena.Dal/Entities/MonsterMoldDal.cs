
using FastArena.Core.Domain.Monsters;

namespace FastArena.Dal.Entities;

public class MonsterMoldDal
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int RankLevel { get; set; } = 1;
    public int BaseHealth { get; set; } = 100;
    public int HealthPerLevel { get; set; } = 10;
    public MonsterSex Sex { get; set; } = MonsterSex.NONE;
    public Guid? PortraitId { get; set; }
    public PortraitDal? Portrait { get; set; }
}
