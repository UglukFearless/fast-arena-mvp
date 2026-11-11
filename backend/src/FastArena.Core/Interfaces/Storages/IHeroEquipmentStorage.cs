using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Interfaces.Storages;

public interface IHeroEquipmentStorage
{
    Task EquipItemAsync(Guid heroId, Guid heroItemCellId, IReadOnlyList<Guid> slotIds);
    Task UnequipItemAsync(Guid heroId, Guid heroItemCellId);
    Task<HeroItemCell> ConsumePocketItemAsync(Guid heroId, Guid heroItemCellId);
}
