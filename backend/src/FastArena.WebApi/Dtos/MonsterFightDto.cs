
namespace FastArena.WebApi.Dtos;

public class MonsterFightDto
{
    public HeroDto Hero { get; set; }
    public MonsterDto Monster { get; set; }
    public Dictionary<int, MonsterFightActionStateDto> State { get; set; }
    public MonsterFightRewardDto? Reward {  get; set; } 
}
