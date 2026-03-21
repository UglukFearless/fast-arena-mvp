

using FastArena.Core.Domain.Activities.Actions;
using FastArena.Core.Domain.MondterFights;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal class MonsterFightProfile
{
    public static MonsterFightDto Map(MonsterFight monsterFight)
    {
        if (monsterFight == null)
            return null;

        return new MonsterFightDto
        {
            Hero = HeroProfile.Map(monsterFight.Hero, true),
            Monster = MonsterProfile.Map(monsterFight.Monster, true),
            State = Map(monsterFight.State),
            Reward = Map(monsterFight.Reward),
        };
    }

    public static Dictionary<int, MonsterFightActionStateDto> Map(Dictionary<int, MonsterFightActionState> state)
    {
        return state.Select(s =>
        {
            return new KeyValuePair<int, MonsterFightActionStateDto>(
                s.Key,
                Map(s.Value));
        }).ToDictionary();
    }

    public static MonsterFightActionStateDto Map(MonsterFightActionState monsterFightActionState)
    {
        if (monsterFightActionState == null)
            return null;

        return new MonsterFightActionStateDto
        {
            HeroHealth = monsterFightActionState.HeroHealth,
            HeroAbility = monsterFightActionState.HeroAbility,
            HeroDiceRoll = monsterFightActionState.HeroDiceRoll,
            MonsterHealth = monsterFightActionState.MonsterHealth,
            MonsterAbility = monsterFightActionState.MonsterAbility,
            MonsterDiceRoll = monsterFightActionState.MonsterDiceRoll,
            Result = monsterFightActionState.Result,
            ActVariants = monsterFightActionState.ActVariants,
        };
    }

    public static MonsterFightResultDto Map(MonsterFightResult model, bool deep = false)
    {
        if (model == null)
            return null;

        var domain = new MonsterFightResultDto
        {
            Id = model.Id,
            HeroId = model.HeroId,
            Order = model.Order,
            Type = model.Type,
            Monster = deep ? MonsterProfile.Map(model.Monster) : null,
        };

        return domain;
    }

    public static List<MonsterFightResultDto> Map(List<MonsterFightResult> models, bool deep = false)
        => models?.ConvertAll(d => Map(d, deep));

    public static MonsterFightRewardDto Map(MonsterFightReward model)
    {
        if (model == null)
            return null;

        return new MonsterFightRewardDto
        {
            Items = ItemProfile.Map(model.Items),
        };
    }

    public static MonsterFightRoundResultDto Map(MonsterFightRoundResult model)
    {
        if (model == null)
            return null;

        var result = new MonsterFightRoundResultDto
        {
            ShoudGoNext = model.ShoudGoNext,
            StateOrder = model.StateOrder,
            State = Map(model.State),
            Reward = Map(model.Reward),
        };

        return result;
    }
}
