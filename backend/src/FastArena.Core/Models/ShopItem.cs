namespace FastArena.Core.Models;

/// <summary>
/// Represents an item available for purchase with calculated sell price.
/// Used internally by ShopService to return data to the API layer.
/// The API layer converts this to ShopItemDto for the client.
/// </summary>
public class ShopItem
{
    public Guid ItemId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int SellPrice { get; set; }
    public required string ItemImage { get; set; }
    public bool CanBeFolded { get; set; }
}
