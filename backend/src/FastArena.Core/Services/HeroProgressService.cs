using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Interfaces.App;

namespace FastArena.Core.Services;

public class HeroProgressService : IHeroProgressService
{
    private int LEVEL_EXPERIENCE_BASE = 100;
    private int FIGHT_EXPERIENCE_BASE = 40;

    public Task<int> CalcExperienceForFightAsync(MonsterFight fight)
    {
        var heroLevel = fight.Hero.Level;
        var heroHealth = fight.Hero.MaxHealth;
        var monsterLevel = fight.Monster.Level;
        var monsterHealth = fight.Monster.MaxHealth;

        return Task.FromResult((int) 
            (FIGHT_EXPERIENCE_BASE * ((float) monsterLevel/heroLevel) * ((float) monsterHealth/heroHealth)));
    }

    public async Task<HeroLevelProgressInfo> GetInfoByLevelAsync(int level)
    {
        if (level <= 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        return await Task.FromResult(new HeroLevelProgressInfo { 
            PreviousAmound = CalcExperenceAmound(level - 1), 
            NextAmound = CalcExperenceAmound(level),
        });
    }

    private long CalcExperenceAmound(int level)
    {
        if (level == 0)
            return 0;
        else
        {
            return LEVEL_EXPERIENCE_BASE * level + CalcExperenceAmound(level - 1);
        }
    }
}
