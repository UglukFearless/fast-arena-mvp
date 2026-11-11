namespace FastArena.Core.Interfaces.App;

public interface IHeroEquipmentService
{
    Task EquipAsync(Guid userId, Guid heroItemCellId);
    Task UnequipAsync(Guid userId, Guid heroItemCellId);
}
