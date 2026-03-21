using FastArena.Core.Domain;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;

namespace FastArena.Core.Services;

public class UserService : IUserService
{
    private readonly IUserStorage _userStorage;
    public UserService(IUserStorage userStorage)
    {
        _userStorage = userStorage;
    }
    public async Task<User> CreateAsyc(string login, string password)
    {
        return await _userStorage.CreateAsync(new UserCreationModel(login, password));
    }

    public async Task<User> GetAsync(Guid id)
    {
        return await _userStorage.GetAsync(id);
    }

    public async Task<User> GetByLoginAndPasswordAsync(string login, string password)
    {
        return await _userStorage.GetByLoginAndPasswordAsync(login, password);
    }
}
