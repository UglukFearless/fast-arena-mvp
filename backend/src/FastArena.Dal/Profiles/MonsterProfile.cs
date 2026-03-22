
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Monsters;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal class MonsterProfile
{
    public static MonsterMold Map(MonsterMoldDal dal, bool deep = false)
    {
        if (dal == null)
            return null;

        var domain = new MonsterMold
        {
            Id = dal.Id,
            Name = dal.Name,
            Sex = dal.Sex,
            BaseHealth = dal.BaseHealth,
            RankLevel = dal.RankLevel,
            HealthPerLevel = dal.HealthPerLevel,
            PortraitId = dal.PortraitId.Value,
            Portrait = deep ? PortraitProfile.Map(dal.Portrait) : null,
        };

        return domain;
    }

    public static List<MonsterMold> Map(List<MonsterMoldDal> dals, bool deep = false) => dals?.ConvertAll(d => Map(d, deep));
}
