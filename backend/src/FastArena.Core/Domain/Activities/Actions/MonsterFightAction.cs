
using FastArena.Core.Domain.Activities.Datas;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Interfaces.App;

namespace FastArena.Core.Domain.Activities.Actions;

public class MonsterFightAction : ActivityAction
{
    public MonsterFightAction()
    {
        Type = ActivityActionType.MONSTER_FIGHT;
    }

    public override async Task<ActivityActionCase> СreateCaseAsync(Hero hero, Activity context, IMonsterService monsterService)
    {
        var midMonsterRank = CalcMonsterRank(hero.Level, context.DangerLevel);
        var monsterMold = await monsterService.GetRandomMoldForRankAsync(midMonsterRank);
        var realMonsterLevel = GenerateRandomLevel(hero.Level, context.DangerLevel);
        var actionCase = new ActivityActionCase()
        {
            Type = ActivityActionType.MONSTER_FIGHT,
            Data = new MonsterFightData()
            {
                Monster = await monsterService.CreateFromMoldAsync(realMonsterLevel, monsterMold),
            }
        };
        return actionCase;
    }

    private int CalcMonsterRank(int level, ActivityDangerLevel dangerLevel) => dangerLevel switch
    {
        ActivityDangerLevel.LOW => (level - 5) < 1 ? 1 : (level - 5),
        ActivityDangerLevel.MEDIUM => level,
        ActivityDangerLevel.HIGH => level + 5,
        _ => throw new NotImplementedException(),
    };

    private int GenerateRandomLevel(int level, ActivityDangerLevel dangerLevel)
    {
        Random random = new Random();
        var resultLevel = level;

        switch (dangerLevel)
        {
            case ActivityDangerLevel.LOW:
                resultLevel = level + random.Next(-7, 1);
                break;
            case ActivityDangerLevel.MEDIUM:
                resultLevel = level + random.Next(-3, 3);
                break;
            case ActivityDangerLevel.HIGH:
                resultLevel = level + random.Next(-2, 6);
                break;
        }

        return resultLevel > 1 ? resultLevel : 1;
    }
}
