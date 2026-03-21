
using FastArena.Core.Domain.Items;

namespace FastArena.WebApi.Dtos;

public class ItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int BaseCost { get; set; }
    public required string ItemImage { get; set; }
    public bool CanBeEquipped { get; set; }
    public bool CanBeFolded { get; set; }
    public ItemType Type { get; set; }
}
