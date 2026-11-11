namespace FastArena.WebApi.Dtos;

public class HeroPocketSlotDto
{
    public Guid Id { get; set; }
    public required string SlotKey { get; set; }
    public HeroItemCellDto? Item { get; set; }
}
