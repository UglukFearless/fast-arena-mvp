using FastArena.Dal;
using FastArena.WebHost.Services.Seeders.Data;

namespace FastArena.WebHost.Services.Seeders;

public class EntityLinker
{
    private readonly ApplicationContext _context;

    public EntityLinker(ApplicationContext context)
    {
        _context = context;
    }

    public async Task LinkMonstersToPortraitsAsync()
    {
        var monsters = _context.MonsterMolds.ToList();
        var portraits = _context.Portraits.ToList();

        foreach (var monster in monsters)
        {
            if (MonsterSeedData.PortraitFileByName.TryGetValue(monster.Name, out var portraitFileName))
            {
                var portrait = portraits.FirstOrDefault(p =>
                    p.Url != null && p.Url.EndsWith(portraitFileName, StringComparison.OrdinalIgnoreCase));

                if (portrait != null)
                {
                    monster.PortraitId = portrait.Id;
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}
