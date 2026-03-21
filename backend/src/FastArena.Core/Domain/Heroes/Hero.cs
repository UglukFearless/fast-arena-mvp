using FastArena.Core.Domain.MondterFights;

namespace FastArena.Core.Domain.Heroes;

public class Hero
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required HeroSex Sex { get; set; }

    public required int Level { get; set; }

    public required long Experience { get; set; }

    public HeroLevelProgressInfo? LevelProgressInfo { get; set; }

    public Portrait? Portrait { get; set; }

    public HeroAliveState IsAlive { get; set; } = HeroAliveState.ALIVE;

    public int MaxHealth { get; set; } = 100;

    public Guid UserId { get; set; }

    public User? User { get; set; }
    public List<HeroItemCell>? Items { get; set; }
    public List<MonsterFightResult>? Results { get; set; }
}
