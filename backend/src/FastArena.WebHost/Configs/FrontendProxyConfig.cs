namespace FastArena.WebHost.Configs;

public static class FrontendProxyConfig
{
    public static IServiceCollection AddFrontendProxy(this IServiceCollection services, IConfiguration configuration)
    {
        var reverseProxyConfig = configuration.GetSection("ReverseProxy");
        services.AddReverseProxy().LoadFromConfig(reverseProxyConfig);
        return services;
    }

    public static IEndpointRouteBuilder AddFrontendProxyRouting(this IEndpointRouteBuilder builder)
    {
        builder.MapReverseProxy();
        return builder;
    }
}
