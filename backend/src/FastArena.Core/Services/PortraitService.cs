using FastArena.Core.Domain;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;

namespace FastArena.Core.Services;

public class PortraitService : IPortraitService
{
    private readonly IPortraitStorage _portraitStorage;
    public PortraitService(IPortraitStorage portraitStorage)
    {
        _portraitStorage = portraitStorage;
    }

    public async Task<IList<Portrait>> GetAllForHeroes()
    {
        return await _portraitStorage.GetByTagName("Heroes");
    }
}
