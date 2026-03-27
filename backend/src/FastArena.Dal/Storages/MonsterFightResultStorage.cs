
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;
using FastArena.Dal.Entities;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class MonsterFightResultStorage : IMonsterFightResultStorage
{
    private ApplicationContext _context;
    public MonsterFightResultStorage(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<MonsterFightResult> CreateAsync(MonsterFightResultCreationModel model)
    {
        var lastOrder = await GetLastResultOrderForHeroId(model.HeroId);

        var result = new MonsterFightResultDal
        {
            Id = Guid.NewGuid(),
            HeroId = model.HeroId,
            Order = (lastOrder + 1),
            Type = model.Type,
            MonsterId = model.Monster.Id,
            MonsterName = model.Monster.Name,
            MonsterMaxHealth = model.Monster.MaxHealth,
            MonsterLevel = model.Monster.Level,
            MonsterPortraitId = model.Monster.Portrait.Id,
            MonsterMoldId = model.Monster.MonsterMoldId,
        };

        _context.MonsterFightResults.Add(result);
        await _context.SaveChangesAsync();

        return await GetAsync(result.Id);
    }

    public async Task<MonsterFightResult> GetAsync(Guid id)
    {
        var result = await _context.MonsterFightResults
            .Include(r => r.Hero)
            .Include(r => r.Portrait)
            .Include(r => r.MonsterMold)
            .FirstAsync(r =>  r.Id == id);

        return MonsterFightProfile.Map(result, true);
    }

    public async Task<ICollection<MonsterFightResult>> GetByHeroIdAsync(Guid heroId)
    {
        var results = await _context.MonsterFightResults
            .Include(r => r.Hero)
            .Include(r => r.Portrait)
            .Include(r => r.MonsterMold)
            .Where(r => r.HeroId == heroId)
            .OrderBy(r => r.Order)
            .ToListAsync();

        return MonsterFightProfile.Map(results, true);
    }

    private async Task<int> GetLastResultOrderForHeroId(Guid heroId)
    {
        var lastResult = await _context.MonsterFightResults
            .Where(r => r.HeroId == heroId).OrderByDescending(r => r.Order).FirstOrDefaultAsync();

        if (lastResult == null)
        {
            return 0;
        }
        return lastResult.Order;
    }
}
