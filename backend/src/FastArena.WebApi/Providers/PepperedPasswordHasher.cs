
using FastArena.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FastArena.WebApi.Providers;

public class PepperedPasswordHasher
{
    private readonly PasswordHasher<User> _innerHasher = new();
    private readonly string _pepper;

    public PepperedPasswordHasher(IConfiguration configuration)
    {
        var paper = configuration.GetValue<string>("AuthOptions:HashSecret");
        if (string.IsNullOrWhiteSpace(paper))
            throw new ArgumentNullException(nameof(paper));
        _pepper = paper;
    }

    public string HashPassword(string password)
    {
        return _innerHasher.HashPassword(null, password + _pepper);
    }

    public bool VerificatePassword(string providedPassword, string storedHash)
    {
        var result = _innerHasher.VerifyHashedPassword(null, storedHash, providedPassword + _pepper);
        return result != PasswordVerificationResult.Failed ? true : false;
    }
}
