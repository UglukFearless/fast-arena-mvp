using FastArena.Core.Domain.Items;

namespace FastArena.Core.Interfaces.Storages;

public interface IItemStorage
{
    Task<Item> GetBaseMoneyItemAsync();
}
