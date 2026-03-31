using FastArena.Core.Models;

namespace FastArena.Core.Interfaces.App;

public interface IHeroInfoService
{
    Task<HeroInfoModel> GetAsync(Guid heroId, Guid requestingUserId);
}
