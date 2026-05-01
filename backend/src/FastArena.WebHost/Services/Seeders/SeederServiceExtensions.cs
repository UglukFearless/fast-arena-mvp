namespace FastArena.WebHost.Services.Seeders;

public static class SeederServiceExtensions
{
    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        services.AddScoped<PortraitSeeder>();
        services.AddScoped<MonsterSeeder>();
        services.AddScoped<EntityLinker>();
        services.AddScoped<ItemSeeder>();
        services.AddScoped<MonsterRewardSeeder>();
        services.AddScoped<SeederRunner>();
        return services;
    }
}
