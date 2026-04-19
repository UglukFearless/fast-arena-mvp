using FastArena.Core.Domain.Effects;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class EffectDefinitionProfiles
{
    public static EffectDefinition? Map(EffectDefinitionDal dal)
    {
        if (dal == null)
            return null;

        return new EffectDefinition
        {
            Id = dal.Id,
            ItemId = dal.ItemId,
            Type = dal.Type,
            DurationRounds = dal.DurationRounds,
            Magnitude = dal.Magnitude,
            MinValue = dal.MinValue,
            MaxValue = dal.MaxValue,
            ChancePercent = dal.ChancePercent,
            ConditionType = dal.ConditionType,
            TargetType = dal.TargetType,
            Priority = dal.Priority,
            NextEffectDefinitionId = dal.NextEffectDefinitionId,
        };
    }

    public static List<EffectDefinition>? Map(List<EffectDefinitionDal> dals) => dals?.ConvertAll(d => Map(d)!);
}
