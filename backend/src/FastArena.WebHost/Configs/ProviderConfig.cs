using FastArena.Core.Domain;
using FastArena.WebApi.Providers;
using Microsoft.AspNetCore.Identity;

namespace FastArena.WebHost.Configs;

public static class ProviderConfig
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<PepperedPasswordHasher>();
        services.AddScoped<AuthProvider>();

        return services;
    }
}
