namespace FastArena.WebApi.Dtos;

public class ShopHeroItemDto
{
    public Guid HeroItemCellId { get; set; }
    public Guid ItemId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ItemImage { get; set; }
    public int Amount { get; set; }
    public bool CanBeFolded { get; set; }
    public int BuyPrice { get; set; }
}
