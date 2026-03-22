using FastArena.Dal;
using Microsoft.EntityFrameworkCore;

namespace FastArena.WebHost.Configs;

public static class DataBaseConfig
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(defaultConnectionString))
        {
            throw new ArgumentNullException(nameof(defaultConnectionString));
        }
        
        //services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(defaultConnectionString));
        services.AddDbContextFactory<ApplicationContext>(options => options.UseNpgsql(defaultConnectionString));
        
        return services;
    }
}
