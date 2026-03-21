
using FastArena.Core.Domain.Items;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;

namespace FastArena.Core.Services;

public class ItemService : IItemService
{
    private readonly IItemStorage _itemStorage;
    public ItemService(IItemStorage itemStorage)
    {
        _itemStorage = itemStorage;
    }
    public async Task<Item> GetBaseMoneyItemAsync()
    {
        return await _itemStorage.GetBaseMoneyItemAsync();
    }
}
