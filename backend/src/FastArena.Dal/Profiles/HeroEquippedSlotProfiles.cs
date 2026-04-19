using FastArena.Core.Domain.Heroes;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class HeroEquippedSlotProfiles
{
    public static HeroEquippedSlot Map(HeroEquippedSlotDal dal, bool deep = false)
    {
        if (dal == null)
        {
            return null;
        }

        return new HeroEquippedSlot
        {
            HeroId = dal.HeroId,
            Slot = dal.Slot,
            HeroItemCellId = dal.HeroItemCellId,
            Hero = null,
            HeroItemCell = deep && dal.HeroItemCell != null ? HeroItemCellProfiles.Map(dal.HeroItemCell, true) : null,
        };
    }

    public static List<HeroEquippedSlot> Map(List<HeroEquippedSlotDal> dals, bool deep = false)
        => dals?.ConvertAll(d => Map(d, deep));
}