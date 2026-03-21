using FastArena.Core.Domain;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class UserProfile
{
    public static UserDto Map(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            SelectedHeroId = user.SelectedHeroId,
        };
    }
}
