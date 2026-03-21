
namespace FastArena.Core.Domain;

public class FightResult
{
    public Guid Id { get; set; }
    public FightResultType Type { get; set; }
    public Guid HeroId { get; set; }
    public Guid? EnemyNpcId { get; set; }
}
