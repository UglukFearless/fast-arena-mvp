using FastArena.WebHost.Configs;
using FastArena.WebHost.Services.Seeders;
using System.Text.Json.Serialization;


namespace FastArena.WebHost;

public class Startup
{
    private readonly IConfigurationRoot _configuration;

    public Startup(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IConfiguration>(_configuration);

        services.AddDataBase(_configuration);
        services.AddStorages();
        services.AddAppServices();
        services.AddProviders();

        services.AddScoped<PortraitSeeder>();
        services.AddScoped<MonsterSeeder>();
        services.AddScoped<EntityLinker>();
        services.AddScoped<SeederRunner>();

        services.AddCors(options =>
        {
            options.AddPolicy("DevCors", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        services.AddAuthConfig(_configuration);

        services.AddEndpointsApiExplorer();
        services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddFrontendProxy(_configuration);
        services.AddHttpContextAccessor();
        
        services.AddSwaggerCongigurations();
    }

public void Configure(WebApplication app)
    {
        app.AddSwaggerConfigurations();

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseCors("DevCors");

        app.UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.AddFrontendProxyRouting();
        });

        // Seed data
        SeedData(app);
    }

    private void SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seederRunner = scope.ServiceProvider.GetRequiredService<SeederRunner>();
        var env = app.Environment;
        
        var wwwrootPath = Path.Combine(env.ContentRootPath, "wwwroot");
        seederRunner.RunAsync(wwwrootPath).GetAwaiter().GetResult();
    }
}
