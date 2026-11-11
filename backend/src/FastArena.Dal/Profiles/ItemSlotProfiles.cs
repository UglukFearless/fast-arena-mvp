using FastArena.Core.Domain.Items;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class ItemSlotProfiles
{
    public static ItemSlot Map(ItemSlotDal dal)
    {
        if (dal == null)
            return null;

        return new ItemSlot
        {
            Id = dal.Id,
            ItemId = dal.ItemId,
            SlotKey = dal.SlotKey,
        };
    }

    public static List<ItemSlot> Map(List<ItemSlotDal> dals)
        => dals?.ConvertAll(Map);
}
