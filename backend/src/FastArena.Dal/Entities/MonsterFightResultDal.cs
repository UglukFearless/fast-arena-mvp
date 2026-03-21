
using FastArena.Core.Domain.MondterFights;

namespace FastArena.Dal.Entities;

public class MonsterFightResultDal
{
    public Guid Id { get; set; }
    public Guid HeroId { get; set; }
    public int Order {  get; set; }
    public MonsterFightResultType Type { get; set; }
    public Guid MonsterId { get; set; }
    public required string MonsterName { get; set; }
    public int MonsterMaxHealth { get; set; }
    public int MonsterLevel { get; set; }
    public Guid MonsterPortraitId { get; set; }
    public Guid MonsterMoldId { get; set; }

    public PortraitDal Portrait { get; set; }
    public HeroDal Hero { get; set; }
    public MonsterMoldDal MonsterMold { get; set; }
}
