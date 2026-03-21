using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FastArena.WebHost.Configs;

public static class AuthConfig
{
    public static AuthenticationBuilder AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration.GetSection("AuthOptions");
        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = authOptions["Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = GetSymmetricSecurityKey(authOptions["SecretKey"]),
                    ValidateIssuerSigningKey = true,
                };
            });
    }

    private static SymmetricSecurityKey GetSymmetricSecurityKey(string secretKey)
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
    }
}
