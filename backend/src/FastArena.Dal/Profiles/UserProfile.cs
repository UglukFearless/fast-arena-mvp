
using FastArena.Core.Domain;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class UserProfile
{
    public static User Map(UserDal userDal)
    {
        if (userDal == null)
            return null;

        return new User
        {
            Id = userDal.Id,
            Login = userDal.Login,
            PasswordHash = userDal.PasswordHash,
            SelectedHeroId = userDal.SelectedHeroId,
        };
    }
}
