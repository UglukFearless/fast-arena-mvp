using FastArena.Core.Models;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class ShopHeroItemProfile
{
    public static ShopHeroItemDto Map(ShopHeroItem item)
    {
        return new ShopHeroItemDto
        {
            HeroItemCellId = item.HeroItemCellId,
            ItemId = item.ItemId,
            Name = item.Name,
            Description = item.Description,
            ItemImage = item.ItemImage,
            Amount = item.Amount,
            CanBeFolded = item.CanBeFolded,
            BuyPrice = item.BuyPrice,
        };
    }

    public static List<ShopHeroItemDto> Map(IEnumerable<ShopHeroItem> items) => items?.Select(Map).ToList();
}
