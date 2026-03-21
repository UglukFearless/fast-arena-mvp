
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.Storages;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class MonsterMoldStorage : IMonsterMoldStorage
{
    private readonly ApplicationContext _context;
    public MonsterMoldStorage(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<ICollection<MonsterMold>> GetAllAsync()
    {
        var monsterMolds = await _context.MonsterMolds.Include(mm => mm.Portrait).ToListAsync();
        return MonsterProfile.Map(monsterMolds, true);
    }

    public async Task<MonsterMold> GetAsync(Guid id)
    {
        var monsterMold = await _context.MonsterMolds.FirstAsync(mm => mm.Id == id);
        return MonsterProfile.Map(monsterMold, true);
    }

    public async Task<ICollection<MonsterMold>> GetFiltered(int maxRank)
    {
        var monsterMolds = await _context.MonsterMolds.Where( mm => mm.RankLevel <= maxRank).ToListAsync();
        return MonsterProfile.Map(monsterMolds, true);
    }
}
