using FastArena.Core.Domain.Heroes;

namespace FastArena.WebApi.Dtos;

public class HeroInfoDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required HeroSex Sex { get; set; }
    public required int Level { get; set; }
    public string? PortraitUrl { get; set; }
    public HeroAliveState IsAlive { get; set; }
    public int MaxHealth { get; set; }
    public int MaxAbility { get; set; }
    public List<MonsterFightResultDto> Results { get; set; } = new List<MonsterFightResultDto>();
}
