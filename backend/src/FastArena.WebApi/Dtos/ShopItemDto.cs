namespace FastArena.WebApi.Dtos;

/// <summary>
/// Represents an item available for purchase in the shop.
/// Contains pre-calculated sell price (+50% markup from base cost).
/// BaseCost is not exposed to frontend to prevent price manipulation.
/// </summary>
public class ShopItemDto
{
    public Guid ItemId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int SellPrice { get; set; }
    public required string ItemImage { get; set; }
    public bool CanBeFolded { get; set; }
}
