using FastArena.Core.Domain.Monsters;
using FastArena.Dal.Entities;

namespace FastArena.WebHost.Services.Seeders.Data;

public static class MonsterSeedData
{
    public static readonly Guid GnomId        = Guid.Parse("37337208-10eb-4367-ae01-6c8e389336af");
    public static readonly Guid BullyId       = Guid.Parse("0affac7f-367d-44de-a0e4-850ba40f5947");
    public static readonly Guid DevilId       = Guid.Parse("b9683eb9-abae-416b-9b14-4747aaedce9e");
    public static readonly Guid FairyId       = Guid.Parse("540345bb-e392-4981-84e6-52822696cdab");
    public static readonly Guid SlugId        = Guid.Parse("15eefcb5-5a35-46da-a30e-26b171aa7ab9");
    public static readonly Guid GolemId       = Guid.Parse("adb57c1e-9dc6-4736-a5f8-be2b311fca71");
    public static readonly Guid MemberbotId   = Guid.Parse("0b1cb7b4-d983-4853-a65d-b9b2c21f4df9");
    public static readonly Guid KaloidId      = Guid.Parse("198d23a7-4fdf-4092-8ce9-451327d9f36a");
    public static readonly Guid RoosterId     = Guid.Parse("09ac9fd3-59f6-413c-a202-8c4d6892717b");
    public static readonly Guid ZombieId      = Guid.Parse("1bbb40d9-258e-44b6-a9d2-caf941961a15");
    public static readonly Guid GhostId       = Guid.Parse("ab96b719-c7fc-420e-bfd8-beb35cf533b6");
    public static readonly Guid DragonId      = Guid.Parse("4e3fb9c4-a451-4cb8-acb1-2030901d8adc");
    public static readonly Guid TrogloditeId  = Guid.Parse("9191d55d-2bfc-4aa8-b064-3a909d666ddc");
    public static readonly Guid WerewolfId    = Guid.Parse("1e309681-9a46-4fc6-822f-94f2b8b23a26");

    public static IReadOnlyList<MonsterMoldDal> All => new List<MonsterMoldDal>
    {
        new() { Id = GnomId,       Name = "Гном-хуекрад",     RankLevel = 3,  HealthPerLevel = 7,  BaseHealth = 50,  Sex = MonsterSex.MALE   },
        new() { Id = BullyId,      Name = "Здоровяк хуелом",  RankLevel = 12, HealthPerLevel = 10, BaseHealth = 120, Sex = MonsterSex.MALE   },
        new() { Id = DevilId,      Name = "Чёрт",             RankLevel = 10, HealthPerLevel = 10, BaseHealth = 110, Sex = MonsterSex.MALE   },
        new() { Id = FairyId,      Name = "Фея хуевёртка",   RankLevel = 3,  HealthPerLevel = 7,  BaseHealth = 50,  Sex = MonsterSex.FEMALE },
        new() { Id = SlugId,       Name = "Жирослизень",      RankLevel = 5,  HealthPerLevel = 8,  BaseHealth = 70,  Sex = MonsterSex.MALE   },
        new() { Id = GolemId,      Name = "Обоссаный голем",  RankLevel = 15, HealthPerLevel = 12, BaseHealth = 150, Sex = MonsterSex.MALE   },
        new() { Id = MemberbotId,  Name = "Членобот",         RankLevel = 8,  HealthPerLevel = 9,  BaseHealth = 90,  Sex = MonsterSex.MALE   },
        new() { Id = KaloidId,     Name = "Калоид",           RankLevel = 13, HealthPerLevel = 12, BaseHealth = 140, Sex = MonsterSex.MALE   },
        new() { Id = RoosterId,    Name = "Петух",            RankLevel = 1,  HealthPerLevel = 5,  BaseHealth = 30,  Sex = MonsterSex.MALE   },
        new() { Id = ZombieId,     Name = "Зомби",            RankLevel = 7,  HealthPerLevel = 10, BaseHealth = 80,  Sex = MonsterSex.MALE   },
        new() { Id = GhostId,      Name = "Призрак",          RankLevel = 6,  HealthPerLevel = 10, BaseHealth = 70,  Sex = MonsterSex.MALE   },
        new() { Id = DragonId,     Name = "Дракон хуеглот",   RankLevel = 20, HealthPerLevel = 15, BaseHealth = 200, Sex = MonsterSex.MALE   },
        new() { Id = TrogloditeId, Name = "Трогладит",        RankLevel = 10, HealthPerLevel = 10, BaseHealth = 100, Sex = MonsterSex.MALE   },
        new() { Id = WerewolfId,   Name = "Ёборотень",        RankLevel = 12, HealthPerLevel = 10, BaseHealth = 120, Sex = MonsterSex.MALE   },
    };

    public static IReadOnlyDictionary<string, string> PortraitFileByName => new Dictionary<string, string>
    {
        ["Гном-хуекрад"]    = "bad-gnome.png",
        ["Здоровяк хуелом"] = "bully-minion.png",
        ["Чёрт"]            = "evil-minion.png",
        ["Фея хуевёртка"]  = "fairy.png",
        ["Жирослизень"]     = "grasping-slug.png",
        ["Обоссаный голем"] = "ice-golem.png",
        ["Членобот"]        = "megabot.png",
        ["Калоид"]          = "rock-golem.png",
        ["Петух"]           = "rooster.png",
        ["Зомби"]           = "shambling-zombie.png",
        ["Призрак"]         = "spectre.png",
        ["Дракон хуеглот"]  = "spiked-dragon-head.png",
        ["Трогладит"]       = "troglodyte.png",
        ["Ёборотень"]       = "werewolf.png",
    };
}
