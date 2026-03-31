using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Models;

namespace FastArena.Core.Services;

public class HeroInfoService : IHeroInfoService
{
    private readonly IHeroService _heroService;

    public HeroInfoService(IHeroService heroService)
    {
        _heroService = heroService;
    }

    public async Task<HeroInfoModel> GetAsync(Guid heroId, Guid requestingUserId)
    {
        var hero = await _heroService.GetAsync(heroId);

        var isInventoryVisible = hero.UserId == requestingUserId;

        return new HeroInfoModel
        {
            Hero = hero,
            IsInventoryVisible = isInventoryVisible,
            MoneyAmount = isInventoryVisible
                ? hero.Items?.FirstOrDefault(i => i.Item?.Type == ItemType.MONEY)?.Amount ?? 0
                : 0,
            InventoryItems = isInventoryVisible
                ? hero.Items?.ToList() ?? new List<HeroItemCell>()
                : new List<HeroItemCell>(),
        };
    }
}
