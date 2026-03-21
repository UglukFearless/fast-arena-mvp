using FastArena.Core.Domain;
using FastArena.Core.Models;

namespace FastArena.Core.Interfaces.Storages;

public interface IUserStorage
{
    Task<User> CreateAsync(UserCreationModel model);
    Task<User> GetByLoginAsync(string login);
    Task<User> GetAsync(Guid id);
    Task SelectHeroAsync(Guid id, Guid heroId);
    Task UnselectHeroAsync(Guid id);
}
