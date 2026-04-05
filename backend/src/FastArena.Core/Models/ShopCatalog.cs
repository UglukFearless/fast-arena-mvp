using FastArena.Core.Domain.Items;

namespace FastArena.Core.Models;

public class ShopCatalog
{
    public required Item MoneyItem { get; set; }
    public required List<ShopItem> Items { get; set; }
}