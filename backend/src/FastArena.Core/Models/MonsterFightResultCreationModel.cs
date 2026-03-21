
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Domain.Monsters;

namespace FastArena.Core.Models;

public class MonsterFightResultCreationModel
{
    public Guid HeroId { get; set; }
    public Monster Monster { get; set; }
    public MonsterFightResultType Type { get; set; }

}
