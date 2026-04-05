namespace FastArena.Core.Models;

public class ShopTransactionModel
{
    public required List<HeroItemTakeRequest> SellItems { get; set; }
    public required List<ShopBuyRequestItem> BuyItems { get; set; }
}

public class HeroItemTakeRequest
{
    public Guid HeroItemCellId { get; set; }
    public int Quantity { get; set; }
}

public class ShopBuyRequestItem
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}