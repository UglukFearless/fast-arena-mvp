using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Interfaces.Storages;

public interface IHeroEquipmentStorage
{
    Task EquipItemToSlotAsync(Guid heroId, Guid heroItemCellId, EquipmentSlotType slot);
    Task UnequipItemFromSlotAsync(Guid heroId, EquipmentSlotType slot);
}