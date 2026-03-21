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
        var baseManeyItem = await _context.Items.FirstAsync(i => i.Type == ItemType.MONEY && i.BaseCost == 1);
        return ItemProfiles.Map(baseManeyItem);
    }
}
