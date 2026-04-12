using FastArena.Core.Domain.Effects;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class EffectDefinitionProfile
{
    public static EffectDefinitionDto? Map(EffectDefinition model)
    {
        if (model == null)
            return null;

        return new EffectDefinitionDto
        {
            Id = model.Id,
            Type = model.Type,
            DurationRounds = model.DurationRounds,
            Magnitude = model.Magnitude,
            MinValue = model.MinValue,
            MaxValue = model.MaxValue,
            ChancePercent = model.ChancePercent,
            ConditionType = model.ConditionType,
            TargetType = model.TargetType,
            Priority = model.Priority,
            NextEffectDefinitionId = model.NextEffectDefinitionId,
        };
    }

    public static List<EffectDefinitionDto>? Map(List<EffectDefinition> models) => models?.ConvertAll(m => Map(m)!);
}
