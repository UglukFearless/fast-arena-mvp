
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
            PortraitId = dal.PortraitId.GetValueOrDefault(),
            Portrait = deep ? PortraitProfile.Map(dal.Portrait) : null,
            RewardEntries = dal.RewardEntries != null && deep
                ? MapRewardEntries(dal.RewardEntries.ToList())
                : new List<MonsterRewardEntry>(),
        };

        return domain;
    }

    public static List<MonsterMold> Map(List<MonsterMoldDal> dals, bool deep = false) => dals?.ConvertAll(d => Map(d, deep));

    private static List<MonsterRewardEntry> MapRewardEntries(List<MonsterRewardEntryDal> dals)
    {
        return dals.ConvertAll(dal => new MonsterRewardEntry
        {
            MonsterMoldId = dal.MonsterMoldId,
            ItemId = dal.ItemId,
            Item = ItemProfiles.Map(dal.Item, true),
            ChancePercent = dal.ChancePercent,
            Amount = dal.Amount,
        });
    }
}
