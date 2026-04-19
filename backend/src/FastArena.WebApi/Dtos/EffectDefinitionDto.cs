using FastArena.Core.Domain.Effects;

namespace FastArena.WebApi.Dtos;

public class EffectDefinitionDto
{
    public Guid Id { get; set; }
    public EffectType Type { get; set; }
    public int DurationRounds { get; set; }
    public int Magnitude { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int ChancePercent { get; set; }
    public EffectConditionType ConditionType { get; set; }
    public EffectTargetType TargetType { get; set; }
    public int Priority { get; set; }
    public Guid? NextEffectDefinitionId { get; set; }
}
