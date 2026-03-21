
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class HeroItemCellProfiles
{
    public static HeroItemCell Map(HeroItemCellDal dal, bool deep = false)
    {
        if (dal == null)
            return null;

        var item = new HeroItemCell
        {
            Id = dal.Id,
            Amount = dal.Amount,
            ItemId = dal.ItemId,
            HeroId = dal.HeroId,
            Item = deep ? ItemProfiles.Map(dal.Item) : null,
        };

        return item;
    }

    public static List<HeroItemCell> Map(List<HeroItemCellDal> dals, bool deep = false) 
        => dals?.ConvertAll(ic => Map(ic, deep));
}
