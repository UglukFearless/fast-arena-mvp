namespace FastArena.WebApi.Dtos;

public class StatisticDataRowDto
{
    public Guid HeroId { get; set; }
    public required string HeroName { get; set; }
    public required string PortraitUrl { get; set; }
    public required int Value { get; set; }
    public required bool IsAlive { get; set; }
}
