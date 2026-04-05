using FastArena.Core.Domain.Items;

namespace FastArena.Core.Interfaces.Storages;

public interface IItemStorage
{
    Task<Item> GetBaseMoneyItemAsync();
    Task<IEnumerable<Item>> GetByTypeAsync(ItemType type);
    Task<List<Item>> GetByIdsAsync(ICollection<Guid> ids);
}
