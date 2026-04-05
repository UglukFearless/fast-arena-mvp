namespace FastArena.WebApi.Dtos;

public class ShopCatalogDto
{
    public required ItemDto MoneyItem { get; set; }
    public required List<ShopItemDto> Items { get; set; }
}