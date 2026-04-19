using FastArena.Core.Domain.Activities.Actions;

namespace FastArena.WebApi.Dtos;

public class MonsterFightDoActionDto
{
    public HeroActVariant ActVariant { get; set; }
    public MonsterFightDoActionDataDto ActionData { get; set; } = new();
}

public class MonsterFightDoActionDataDto
{
    public Guid? UsedPocketItemCellId { get; set; }
}