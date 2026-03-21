
namespace FastArena.WebApi.Dtos;

public class HeroItemCellDto
{
    public Guid Id { get; set; }
    public Guid HeroId { get; set; }
    public Guid ItemId { get; set; }
    public int Amount { get; set; }
    public ItemDto Item { get; set; }
}
