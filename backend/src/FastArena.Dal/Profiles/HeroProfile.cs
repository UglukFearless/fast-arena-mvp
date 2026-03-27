using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Models;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class HeroProfile
{
    public static Hero Map(HeroDal heroDal, bool deep = false)
    {
        if (heroDal == null)
            return null;

        var hero = new Hero
        {
            Id = heroDal.Id,
            Name = heroDal.Name,
            Sex = heroDal.Sex,
            Level = heroDal.Level,
            Experience = heroDal.Experience,
            IsAlive = heroDal.IsAlive,
            Portrait = deep ? PortraitProfile.Map(heroDal.Portrait) : null,
            MaxHealth = heroDal.MaxHealth,
            UserId = heroDal.UserId,
            Items = deep ? HeroItemCellProfiles.Map(heroDal.Items?.ToList(), true) : new List<HeroItemCell>(),
            Results = deep ? MonsterFightProfile.Map(heroDal.Results?.ToList(), true) : new List<MonsterFightResult>(),
        };

        return hero;
    }

    public static List<Hero> Map(List<HeroDal> heroes, bool deep = false) => heroes?.ConvertAll(h => Map(h, deep));

    public static HeroDal Map(HeroCreationModel heroModel)
    {
        if (heroModel == null)
            return null;

        var hero = new HeroDal
        {
            Id = Guid.NewGuid(),
            Name = heroModel.Name,
            Sex = heroModel.Sex,
            Level = 1,
            Experience = 0,
            IsAlive = heroModel.IsAlive,
            PortraitId = heroModel.PortraitId,
            MaxHealth = heroModel.MaxHealth,
            UserId = heroModel.UserId,
        };
        return hero;
    }
}
