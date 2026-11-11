using FastArena.Core.Domain.Heroes;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class HeroSlotProfiles
{
    public static HeroSlot Map(HeroSlotDal dal, bool deep = false)
    {
        if (dal == null)
            return null;

        return new HeroSlot
        {
            Id = dal.Id,
            HeroId = dal.HeroId,
            SlotKey = dal.SlotKey,
            HeroItemCellId = dal.HeroItemCellId,
            HeroItemCell = deep && dal.HeroItemCell != null
                ? HeroItemCellProfiles.Map(dal.HeroItemCell, true)
                : null,
        };
    }

    public static List<HeroSlot> Map(List<HeroSlotDal> dals, bool deep = false)
        => dals?.ConvertAll(d => Map(d, deep));

    public static List<HeroSlotDal> Map(List<HeroSlot> slots)
        => slots?.ConvertAll(s => new HeroSlotDal
        {
            Id = s.Id,
            HeroId = s.HeroId,
            SlotKey = s.SlotKey,
            HeroItemCellId = s.HeroItemCellId,
        });
}
