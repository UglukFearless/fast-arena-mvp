using FastArena.Core.Interfaces.Storages;
using FastArena.Dal.Storages;

namespace FastArena.WebHost.Configs;

public static class StorageConfig
{
    public static IServiceCollection AddStorages(this IServiceCollection services)
    {
        services.AddScoped<IHeroStorage, HeroStorage>();
        services.AddScoped<IHeroEquipmentStorage, HeroEquipmentStorage>();
        services.AddScoped<IUserStorage, UserStorage>();
        services.AddScoped<IPortraitStorage, PortraitStorage>();
        services.AddScoped<IItemStorage, ItemStorage>();
        services.AddScoped<IMonsterMoldStorage, MonsterMoldStorage>();
        services.AddScoped<IMonsterFightResultStorage, MonsterFightResultStorage>();

        // In Memory!
        services.AddSingleton<IActivitySessionStorage, ActivitySessionStorage>();

        return services;
    }
}
