using FastArena.Core.Models;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class ShopCatalogProfile
{
    public static ShopCatalogDto Map(ShopCatalog model)
    {
        return new ShopCatalogDto
        {
            MoneyItem = ItemProfile.Map(model.MoneyItem),
            Items = ShopItemProfile.Map(model.Items),
        };
    }
}