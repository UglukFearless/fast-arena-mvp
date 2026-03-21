using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Models;

public class HeroCreationModel()
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public HeroSex Sex { get; set; }
    public Guid PortraitId { get; set; }
    public HeroAliveState IsAlive { get; } = HeroAliveState.ALIVE;
    public int MaxHealth { get; set; } = 100;
}
