using FastArena.Core.Domain;

namespace FastArena.Core.Interfaces.App;

public interface IPortraitService
{
    Task<IList<Portrait>> GetAllForHeroes();
}
