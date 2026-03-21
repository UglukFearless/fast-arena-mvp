using FastArena.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FastArena.WebApi.Providers;

public class AuthProvider
{
    private readonly IConfigurationSection _configuration;
    public AuthProvider(IConfiguration configuration)
    {
        _configuration = configuration.GetSection("AuthOptions");
    }

    public string GetTokenForUser(User user)
    {
        var identity = GetIdentity(user.Login, user.Id);
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(int.Parse(_configuration["Lifetime"]))),
                signingCredentials: new SigningCredentials(
                    GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256)
                );
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    private ClaimsIdentity GetIdentity(string login, Guid id)
    {
        var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                };
        ClaimsIdentity claimsIdentity =
        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }

    private SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["SecretKey"]));
    }

    /// <summary>
    /// Gets current userId from accessor from token
    /// </summary>
    /// <returns>Current user id</returns>
    /// <exception cref="ArgumentException" />
    public static Guid GetCurrentUserIdFromAccessor(IHttpContextAccessor httpContextAccessor)
    {
        var userIdString = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdString);
        return userId;
    }
}
