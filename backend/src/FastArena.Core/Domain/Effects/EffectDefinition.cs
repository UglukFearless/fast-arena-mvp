namespace FastArena.Core.Domain.Effects;

public class EffectDefinition
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public required EffectType Type { get; set; }
    public int DurationRounds { get; set; }
    public int Magnitude { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int ChancePercent { get; set; } = 100;
    public required EffectConditionType ConditionType { get; set; }
    public required EffectTargetType TargetType { get; set; }
    public int Priority { get; set; } = 0;
    public Guid? NextEffectDefinitionId { get; set; }
}
