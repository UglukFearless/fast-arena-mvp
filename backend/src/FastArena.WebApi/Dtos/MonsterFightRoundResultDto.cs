namespace FastArena.WebApi.Dtos;

public class MonsterFightRoundResultDto
{
    public bool ShoudGoNext { get; set; }
    public int? StateOrder { get; set; }
    public MonsterFightActionStateDto? State { get; set; }
    public MonsterFightRewardDto? Reward { get; set; }
}
