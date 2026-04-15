using FastArena.Core.Interfaces.App;
using FastArena.Core.Services;

namespace FastArena.WebHost.Configs;

public static class ServiceConfig
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IHeroService, HeroService>();
        services.AddScoped<IHeroEquipmentService, HeroEquipmentService>();
        services.AddScoped<IHeroInfoService, HeroInfoService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPortraitService, PortraitService>();
        services.AddScoped<IActivityStateService, ActivityStateService>();
        services.AddScoped<IHeroProgressService, HeroProgressService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IActivitySessionService, ActivitySessionService>();
        services.AddScoped<IMonsterService, MonsterService>();
        services.AddScoped<IMonsterFightService, MonsterFightService>();
        services.AddScoped<IMonsterFightResultService, MonsterFightResultService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IStatisticService, StatisticService>();
        services.AddScoped<IShopService, ShopService>();

        return services;
    }
}
