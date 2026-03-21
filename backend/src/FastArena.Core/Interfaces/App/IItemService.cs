using FastArena.Core.Domain.Items;

namespace FastArena.Core.Interfaces.App;

public interface IItemService
{
    Task<Item> GetBaseMoneyItemAsync();
}
