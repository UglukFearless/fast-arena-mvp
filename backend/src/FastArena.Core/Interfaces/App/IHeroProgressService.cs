using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.MondterFights;

namespace FastArena.Core.Interfaces.App;

public interface IHeroProgressService
{
    Task<HeroLevelProgressInfo> GetInfoByLevelAsync(int level);
    Task<int> CalcExperienceForFightAsync(MonsterFight fight);
}
