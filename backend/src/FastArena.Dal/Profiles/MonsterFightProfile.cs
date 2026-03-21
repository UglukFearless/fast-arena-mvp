
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Domain.Monsters;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal class MonsterFightProfile
{
    public static MonsterFightResult Map(MonsterFightResultDal dal, bool deep = false)
    {
        if (dal == null)
            return null;

        var domain = new MonsterFightResult
        {
            Id = dal.Id,
            HeroId = dal.HeroId,
            Order = dal.Order,
            Type = dal.Type,
            Monster = new Monster
            {
                Id = dal.MonsterId,
                Name = dal.MonsterName,
                MaxHealth = dal.MonsterMaxHealth,
                Level = dal.MonsterLevel,
                MonsterMoldId = dal.MonsterMoldId,
                Portrait = dal.Portrait != null && deep ? PortraitProfile.Map(dal.Portrait) : null,
                Mold = dal.MonsterMold != null && deep ? MonsterProfile.Map(dal.MonsterMold) : null,
            }
        };

        return domain;
    }

    public static List<MonsterFightResult> Map(List<MonsterFightResultDal> dals, bool deep = false) 
        => dals?.ConvertAll(d => Map(d, deep));
}
