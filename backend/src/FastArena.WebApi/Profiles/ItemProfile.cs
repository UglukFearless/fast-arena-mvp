
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class ItemProfile
{
    public static ItemDto Map(Item item, bool deep = false)
    {
        if (item == null)
            return null;

        var dto = new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            ItemImage = item.ItemImage,
            CanBeEquipped = item.CanBeEquipped,
            CanBeFolded = item.CanBeFolded,
            BaseCost = item.BaseCost,
            Type = item.Type,
        };

        return dto;
    }

    public static List<ItemDto> Map(List<Item> items, bool deep = false) => items?.ConvertAll(i => Map(i, deep));

    public static GivenItemDto Map(GivenItem model)
    {
        if (model == null)
            return null;

        var dto = new GivenItemDto
        {
            Item = Map(model.Item),
            Amound = model.Amount,
        };

        return dto;
    }

    public static List<GivenItemDto> Map(List<GivenItem> models) => models?.ConvertAll(m => Map(m));
}
