using FastArena.Core.Domain;

namespace FastArena.Core.Interfaces.App;

public interface IUserService
{
    Task<User> CreateAsyc(string login, string password);
    Task<User> GetByLoginAndPasswordAsync(string login, string password);
    Task<User> GetAsync(Guid id);
}
