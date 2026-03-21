using FastArena.Core.Domain;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;
using FastArena.Dal.Entities;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class HeroStorage : IHeroStorage
{
    private ApplicationContext _context;
    public HeroStorage(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Hero> CreateAsync(HeroCreationModel model)
    {
        var newHero = HeroProfile.Map(model);
        _context.Heroes.Add(newHero);
        await _context.SaveChangesAsync();
        return HeroProfile.Map(newHero);
    }

    public async Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId)
    {
        var heroes = await _context.Heroes
            .Where(h => h.UserId == userId)
            .Include(h => h.Portrait)
            .AsNoTracking().ToListAsync();
        return HeroProfile.Map(heroes, true);
    }

    public async Task<ICollection<Hero>> GetAllWithIncludesByUserIdAsync(Guid userId)
    {
        var heroes = await _context.Heroes
            .Where(h => h.UserId == userId)
            .Include(h => h.Portrait)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .Include(h => h.Results)
            .ThenInclude(r => r.Portrait)
            .AsNoTracking()
            .ToListAsync();
        return HeroProfile.Map(heroes, true);
    }

    public async Task<ICollection<Hero>> GetAllWithIncludesAsync()
    {
        var heroes = await _context.Heroes
            .Include(h => h.Portrait)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .Include(h => h.Results)
            .ThenInclude(r => r.Portrait)
            .AsNoTracking()
            .ToListAsync();
        return HeroProfile.Map(heroes, true);
    }

    public async Task<Hero> GetAsync(Guid id)
    {
        var hero = await _context.Heroes
            .Include(h => h.Portrait)
            .Include(h => h.Items)
            .ThenInclude(ic => ic.Item)
            .Include(h => h.Results)
            .ThenInclude(r => r.Portrait)
            .AsNoTracking()
            .FirstAsync(h => h.Id == id);
        return HeroProfile.Map(hero, true);
    }

    public async Task<List<HeroItemCell>> GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items)
    {
        var heroDal = await GetWithItemsAsync(heroId);

        var foldedItems = items.Where(gi => gi.Item.CanBeFolded);
        var notFoldedItems = items.Where(gi => !gi.Item.CanBeFolded);

        foreach(var foldedItem in foldedItems)
        {
            var existItem = heroDal.Items.FirstOrDefault(i => i.ItemId == foldedItem.Item.Id);
            if (existItem != null)
            {
                existItem.Amount += foldedItem.Amount;
            }
            else
            {
                _context.HeroItemCells.Add(new HeroItemCellDal
                {
                    Id = Guid.NewGuid(),
                    HeroId = heroId,
                    ItemId = foldedItem.Item.Id,
                    Amount = foldedItem.Amount,
                });
            }
        }

        foreach(var notFoldedItem in notFoldedItems)
        {
            if (notFoldedItem.Amount == 1)
            {
                _context.HeroItemCells.Add(new HeroItemCellDal
                {
                    Id = Guid.NewGuid(),
                    HeroId = heroId,
                    ItemId = notFoldedItem.Item.Id,
                    Amount = 1,
                });
            } 
            else
            {
                for (int i = 0; i < notFoldedItem.Amount; i++)
                {
                    _context.HeroItemCells.Add(new HeroItemCellDal
                    {
                        Id = Guid.NewGuid(),
                        HeroId = heroId,
                        ItemId = notFoldedItem.Item.Id,
                        Amount = 1,
                    });
                }
            }
        }

        await _context.SaveChangesAsync();

        var newItems = await _context.HeroItemCells.Where(ic => ic.HeroId == heroId).Include(ic => ic.Item).ToListAsync();

        return HeroItemCellProfiles.Map(newItems, true);
    }

    public async Task<Hero> UpdateHeroAsync(Hero hero)
    {
        var storedHero = await _context.Heroes.FirstAsync(h => h.Id == hero.Id);

        storedHero.Name = hero.Name;
        storedHero.Sex = hero.Sex;
        storedHero.Level = hero.Level;
        storedHero.Experience = hero.Experience;
        storedHero.IsAlive = hero.IsAlive;
        storedHero.MaxHealth = hero.MaxHealth;

        await _context.SaveChangesAsync();

        return await GetAsync(hero.Id);
    }

    #region private methods
    private async Task<HeroDal> GetWithItemsAsync(Guid id)
    {
        var hero = await _context.Heroes.Include(h => h.Items).ThenInclude(ic => ic.Item).FirstAsync(h => h.Id == id);
        return hero;
    }
    #endregion private methods
}
