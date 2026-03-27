using FastArena.Core.Domain.Heroes;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

public class HeroInfoProfile
{
    public static HeroInfoDto Map(Hero hero, bool deep = false)
    {
        if (hero == null)
            return null;

        return new HeroInfoDto
        {
            Id = hero.Id,
            Name = hero.Name,
            Sex = hero.Sex,
            Level = hero.Level,
            PortraitUrl = hero.Portrait?.Url,
            MaxHealth = hero.MaxHealth,
            MaxAbility = hero.MaxHealth / 10,
            IsAlive = hero.IsAlive,
            Results = deep ? MonsterFightProfile.Map(hero.Results, true) : new List<MonsterFightResultDto>(),
        };
    }

    public static List<HeroInfoDto> Map(List<Hero> heroes, bool deep = false) => heroes?.ConvertAll(h => Map(h, deep));
}
