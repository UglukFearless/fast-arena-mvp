
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Domain;

namespace FastArena.WebApi.Dtos;

public class MonsterDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int MaxHealth { get; set; }
    public int MaxAbility { get; set; }
    public int Level { get; set; }
    public string PortraitUrl { get; set; }
}
