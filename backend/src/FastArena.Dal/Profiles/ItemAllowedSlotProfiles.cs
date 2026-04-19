using FastArena.Core.Domain.Items;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class ItemAllowedSlotProfiles
{
    public static ItemAllowedSlot Map(ItemAllowedSlotDal dal)
    {
        if (dal == null)
        {
            return null;
        }

        return new ItemAllowedSlot
        {
            ItemId = dal.ItemId,
            Slot = dal.Slot,
            Item = null,
        };
    }

    public static List<ItemAllowedSlot> Map(List<ItemAllowedSlotDal> dals) => dals?.ConvertAll(Map);
}