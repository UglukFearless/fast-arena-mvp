using FastArena.Core.Models;

namespace FastArena.Core.Interfaces.App;

public interface IShopService
{
    /// <summary>
    /// Retrieve available shop items (potions) for a hero with calculated sell prices.
    /// Validates hero ownership and activity state.
    /// </summary>
    /// <param name="userId">Id of the requesting user (from JWT token)</param>
    /// <returns>Shop catalog with money item and sell prices (+50% markup from base cost)</returns>
    /// <exception cref="EntityNotFoundException">No hero selected</exception>
    /// <exception cref="ActionDeniedException">Hero is busy on adventure</exception>
    Task<ShopCatalog> GetAvailableItemsAsync(Guid userId);

    Task<List<ShopHeroItem>> GetHeroItemsAsync(Guid userId);

    Task ExecuteTransactionAsync(Guid userId, ShopTransactionModel model);
}
