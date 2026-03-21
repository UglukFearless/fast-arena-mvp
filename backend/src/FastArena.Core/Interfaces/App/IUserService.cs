using FastArena.Core.Domain;

namespace FastArena.Core.Interfaces.App;

public interface IUserService
{
    Task<User> CreateAsync(string login, string passwordHash);
    Task<User> GetByLoginAsync(string login);
    Task<User> GetAsync(Guid id);
}
