using FastArena.Core.Domain.Items;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class ItemProfiles
{
    public static Item Map(ItemDal dal, bool deep = false)
    {
        if (dal == null)
            return null;

        var item = new Item
        {
            Id = dal.Id,
            Name = dal.Name,
            Description = dal.Description,
            ItemImage = dal.ItemImage,
            CanBeEquipped = dal.CanBeEquipped,
            CanBeFolded = dal.CanBeFolded,
            BaseCost = dal.BaseCost,
            Type = dal.Type,
        };

        return item;
    }

    public static List<Item> Map(List<ItemDal> dals, bool deep = false) => dals?.ConvertAll(i => Map(i, deep));
}
