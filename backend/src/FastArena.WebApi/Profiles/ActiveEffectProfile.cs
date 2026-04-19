using FastArena.Core.Domain.Effects;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class ActiveEffectProfile
{
    public static ActiveEffectDto Map(ActiveEffect effect)
    {
        if (effect == null)
            return null;

        return new ActiveEffectDto
        {
            DefinitionId = effect.DefinitionId,
            Type = effect.Type,
            ImageUrl = effect.SourceImageUrl,
            RemainingRounds = effect.RemainingRounds,
            Magnitude = effect.Magnitude,
            MinValue = effect.MinValue,
            MaxValue = effect.MaxValue,
            ChancePercent = effect.ChancePercent,
            ConditionType = effect.ConditionType,
            TargetType = effect.TargetType,
            Priority = effect.Priority,
            StackCount = effect.StackCount,
        };
    }

    public static List<ActiveEffectDto> Map(List<ActiveEffect> effects)
        => effects?.ConvertAll(Map) ?? new List<ActiveEffectDto>();
}