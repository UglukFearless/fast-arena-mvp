using FastArena.Core.Domain.MondterFights;

namespace FastArena.WebApi.Dtos;

public class MonsterFightResultDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public Guid HeroId { get; set; }
    public MonsterFightResultType Type { get; set; }
    public required MonsterDto Monster { get; set; }
    public HeroDto? Hero { get; set; }
}
