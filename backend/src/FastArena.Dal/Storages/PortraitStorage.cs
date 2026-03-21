

using FastArena.Core.Domain;
using FastArena.Core.Interfaces.Storages;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class PortraitStorage : IPortraitStorage
{
    private ApplicationContext _context;
    public PortraitStorage(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IList<Portrait>> GetByTagName(string tagName)
    {
        var heroTag = _context.PortraitTags
            .Include(tag => tag.Portraits)
            .Where(t => t.Name == tagName)
            .First();


        return PortraitProfile.Map([.. heroTag.Portraits]);
    }
}
