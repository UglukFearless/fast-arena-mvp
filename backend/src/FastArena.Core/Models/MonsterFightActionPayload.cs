using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.Core.Models;

public class MonsterFightActionPayload
{
    public HeroActVariant ActVariant { get; set; }
    public MonsterFightActionData ActionData { get; set; } = new();
}

public class MonsterFightActionData
{
    public Guid? UsedPocketItemCellId { get; set; }
}