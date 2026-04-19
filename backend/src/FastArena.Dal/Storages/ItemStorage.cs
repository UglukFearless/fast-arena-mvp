using FastArena.Core.Domain.Items;
using FastArena.Core.Interfaces.Storages;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class ItemStorage : IItemStorage
{
    private readonly ApplicationContext _context;
    public ItemStorage(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<Item> GetBaseMoneyItemAsync()
    {
        var baseManeyItem = await _context.Items
            .Include(i => i.Effects)
            .FirstAsync(i => i.Type == ItemType.MONEY && i.BaseCost == 1);
        return ItemProfiles.Map(baseManeyItem);
    }


    public async Task<IEnumerable<Item>> GetByTypeAsync(ItemType type)
    {
        var items = await _context.Items
            .Include(i => i.Effects)
            .Where(i => i.Type == type)
            .ToListAsync();
        return items.Select(i => ItemProfiles.Map(i)).ToList();
    }

    public async Task<List<Item>> GetByIdsAsync(ICollection<Guid> ids)
    {
        var items = await _context.Items
            .Include(i => i.Effects)
            .Where(i => ids.Contains(i.Id))
            .ToListAsync();
        return items.Select(i => ItemProfiles.Map(i)).ToList();
    }
}
