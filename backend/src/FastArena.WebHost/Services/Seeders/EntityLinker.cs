using FastArena.Dal;

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
        var monsterToPortraitMap = new Dictionary<string, string>
        {
            ["Гном-хуекрад"] = "bad-gnome.png",
            ["Здоровяк хуелом"] = "bully-minion.png",
            ["Чёрт"] = "evil-minion.png",
            ["Фея хуевёртка"] = "fairy.png",
            ["Жирослизень"] = "grasping-slug.png",
            ["Обоссаный голем"] = "ice-golem.png",
            ["Членобот"] = "megabot.png",
            ["Калоид"] = "rock-golem.png",
            ["Петух"] = "rooster.png",
            ["Зомби"] = "shambling-zombie.png",
            ["Призрак"] = "spectre.png",
            ["Дракон хуеглот"] = "spiked-dragon-head.png",
            ["Трогладит"] = "troglodyte.png",
            ["Ёборотень"] = "werewolf.png",
        };

        var monsters = _context.MonsterMolds.ToList();
        var portraits = _context.Portraits.ToList();

        foreach (var monster in monsters)
        {
            if (monsterToPortraitMap.TryGetValue(monster.Name, out var portraitFileName))
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
