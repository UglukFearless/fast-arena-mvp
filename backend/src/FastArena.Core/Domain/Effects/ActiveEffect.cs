namespace FastArena.Core.Domain.Effects;

/// <summary>
/// Runtime representation of an active effect during combat.
/// Copied from EffectDefinition at activation time; tracks remaining duration and stack count.
/// </summary>
public class ActiveEffect
{
    public Guid DefinitionId { get; set; }
    public required EffectType Type { get; set; }
    public int RemainingRounds { get; set; }
    public int Magnitude { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int ChancePercent { get; set; } = 100;
    public required EffectConditionType ConditionType { get; set; }
    public required EffectTargetType TargetType { get; set; }
    public int Priority { get; set; } = 0;
    public int StackCount { get; set; } = 1;
}
