
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal class MonsterProfile
{
    public static MonsterDto Map(Monster monster, bool deep = false)
    {
        if (monster == null)
            return null;

        return new MonsterDto
        {
            Id = monster.Id,
            Name = monster.Name,
            Level = monster.Level,
            PortraitUrl = monster.Portrait?.Url,
            MaxHealth = monster.MaxHealth,
            MaxAbility = monster.MaxHealth / 10,
        };
    }

    public static List<MonsterDto> Map(List<Monster> monsters, bool deep = false) => monsters?.ConvertAll(h => Map(h, deep));
}
