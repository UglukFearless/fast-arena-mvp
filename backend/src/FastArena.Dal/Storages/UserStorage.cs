using FastArena.Core.Domain;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;
using FastArena.Dal.Entities;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class UserStorage : IUserStorage
{
    private ApplicationContext _context;
    public UserStorage(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<User> CreateAsync(UserCreationModel model)
    {
        var newUser = new UserDal
        {
            Id = Guid.NewGuid(),
            Login = model.Login,
            PasswordHash = model.PasswordHash,
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return UserProfile.Map(newUser);
    }

    public async Task<User> GetByLoginAsync(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        return UserProfile.Map(user);
    }

    public async Task<User> GetAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return UserProfile.Map(user);
    }

    public async Task SelectHeroAsync(Guid id, Guid heroId)
    {
        var doesUserOwnHero = _context.Heroes.Any(h => h.Id == heroId && h.UserId == id);

        if (!doesUserOwnHero)
            throw new ActionDeniedException("The hero doesn't belong to the user.");

        var isHeroAlive = _context.Heroes.Any(h => h.Id == heroId && h.IsAlive == HeroAliveState.ALIVE);

        if (!isHeroAlive)
            throw new ActionDeniedException("The hero is dead.");

        var user = await _context.Users.FirstAsync(u => u.Id == id);
        user.SelectedHeroId = heroId;
        await _context.SaveChangesAsync();
    }

    public async Task UnselectHeroAsync(Guid id)
    {
        var user = await _context.Users.FirstAsync(u => u.Id == id);
        user.SelectedHeroId = null;
        await _context.SaveChangesAsync();
    }
}
