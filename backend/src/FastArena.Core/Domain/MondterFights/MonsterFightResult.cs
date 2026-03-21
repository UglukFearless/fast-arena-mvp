
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Domain.MondterFights;

public class MonsterFightResult
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public Guid HeroId { get; set; }
    public MonsterFightResultType Type { get; set; }
    public required Monster Monster { get; set; }
    public Hero? Hero { get; set; }
}
