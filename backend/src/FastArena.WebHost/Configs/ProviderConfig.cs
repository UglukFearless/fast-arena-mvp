using FastArena.WebApi.Providers;

namespace FastArena.WebHost.Configs;

public static class ProviderConfig
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<AuthProvider>();

        return services;
    }
}
