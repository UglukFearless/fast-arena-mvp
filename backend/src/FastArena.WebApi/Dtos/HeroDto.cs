using FastArena.Core.Domain.Heroes;

namespace FastArena.WebApi.Dtos;

public class HeroDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required HeroSex Sex { get; set; }
    public required int Level { get; set; }
    public required long Experience { get; set; }
    public HeroLevelProgressDto? LevelProgressInfo { get; set; }
    public string? PortraitUrl { get; set; }
    public HeroAliveState IsAlive { get; set; }
    public int MaxHealth { get; set; }
    public int MaxAbility { get; set; }
    public Guid UserId { get; set; }
    public List<HeroItemCellDto> Items { get; set; } = new List<HeroItemCellDto>();
    public List<MonsterFightResultDto> Results { get; set; } = new List<MonsterFightResultDto>();
}
