using FastArena.Core.Models;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class ShopItemProfile
{
    public static ShopItemDto Map(ShopItem item)
    {
        return new ShopItemDto
        {
            ItemId = item.ItemId,
            Name = item.Name,
            Description = item.Description,
            ItemImage = item.ItemImage,
            SellPrice = item.SellPrice,
            CanBeFolded = item.CanBeFolded,
        };
    }

    public static List<ShopItemDto> Map(IEnumerable<ShopItem> items) => items?.Select(Map).ToList();
}
