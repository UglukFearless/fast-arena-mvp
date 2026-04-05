namespace FastArena.WebApi.Models;

public class ShopTransactionModel
{
    public required List<ShopSellRequestItemModel> SellItems { get; set; }
    public required List<ShopBuyRequestItemModel> BuyItems { get; set; }
}

public class ShopSellRequestItemModel
{
    public Guid HeroItemCellId { get; set; }
    public int Quantity { get; set; }
}

public class ShopBuyRequestItemModel
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}