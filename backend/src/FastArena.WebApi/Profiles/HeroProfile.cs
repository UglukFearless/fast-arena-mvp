using FastArena.Core.Domain.Heroes;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal class HeroProfile
{
    public static HeroDto Map(Hero hero, bool deep = false)
    {
        if (hero == null)
            return null;

        return new HeroDto
        {
            Id = hero.Id,
            Name = hero.Name,
            Sex = hero.Sex,
            Level = hero.Level,
            Experience = hero.Experience,
            LevelProgressInfo = Map(hero.LevelProgressInfo),
            PortraitUrl = hero.Portrait?.Url,
            MaxHealth = hero.MaxHealth,
            MaxAbility = hero.MaxHealth / 10,
            IsAlive = hero.IsAlive,
            UserId = hero.UserId,
            Items = deep ? HeroItemCellProfile.Map(hero.Items, true) : new List<HeroItemCellDto>(),
            Results = deep ? MonsterFightProfile.Map(hero.Results, true) : new List<MonsterFightResultDto>(),
        };
    }

    public static List<HeroDto> Map(List<Hero> heroes, bool deep = false) => heroes?.ConvertAll(h => Map(h, deep));

    public static HeroLevelProgressDto Map(HeroLevelProgressInfo info)
    {
        if (info == null)
            return null;

        return new HeroLevelProgressDto
        {
            PreviousAmound = info.PreviousAmound,
            NextAmound = info.NextAmound,
        };
    }
}
