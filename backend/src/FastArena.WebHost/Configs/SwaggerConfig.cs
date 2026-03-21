using NSwag.Generation.Processors.Security;
using NSwag;

namespace FastArena.WebHost.Configs;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerCongigurations(this IServiceCollection services)
    {
        services.AddOpenApiDocument(document =>
        {
            document.Title = "Fast Arena API";
            document.DocumentName = "FastArenaAPI";
            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            document.AddSecurity("JWT", Enumerable.Empty<string>(),
                new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Copy this into the value field: Bearer {token}"
                }
            );
        });

        return services;
    }

    public static WebApplication AddSwaggerConfigurations(this WebApplication app)
    {
        app.UseOpenApi();
        app.UseSwaggerUi();

        return app;
    }
}
