using FastArena.Dal;
using FastArena.Dal.Entities;
using FastArena.Core.Domain.Monsters;
using Microsoft.EntityFrameworkCore;

namespace FastArena.WebHost.Services.Seeders;

public class MonsterSeeder
{
    private readonly ApplicationContext _context;

    public MonsterSeeder(ApplicationContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var monsters = new List<MonsterMoldDal>
        {
            new() { Id = Guid.Parse("37337208-10eb-4367-ae01-6c8e389336af"), Name = "Гном-хуекрад", RankLevel = 3, HealthPerLevel = 5, BaseHealth = 50, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("0affac7f-367d-44de-a0e4-850ba40f5947"), Name = "Здоровяк хуелом", RankLevel = 12, HealthPerLevel = 10, BaseHealth = 120, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("b9683eb9-abae-416b-9b14-4747aaedce9e"), Name = "Чёрт", RankLevel = 10, HealthPerLevel = 10, BaseHealth = 110, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("540345bb-e392-4981-84e6-52822696cdab"), Name = "Фея хуевёртка", RankLevel = 3, HealthPerLevel = 5, BaseHealth = 50, Sex = MonsterSex.FEMALE },
            new() { Id = Guid.Parse("15eefcb5-5a35-46da-a30e-26b171aa7ab9"), Name = "Жирослизень", RankLevel = 5, HealthPerLevel = 8, BaseHealth = 70, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("adb57c1e-9dc6-4736-a5f8-be2b311fca71"), Name = "Обоссаный голем", RankLevel = 15, HealthPerLevel = 12, BaseHealth = 150, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("0b1cb7b4-d983-4853-a65d-b9b2c21f4df9"), Name = "Членобот", RankLevel = 8, HealthPerLevel = 9, BaseHealth = 90, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("198d23a7-4fdf-4092-8ce9-451327d9f36a"), Name = "Калоид", RankLevel = 13, HealthPerLevel = 12, BaseHealth = 140, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("09ac9fd3-59f6-413c-a202-8c4d6892717b"), Name = "Петух", RankLevel = 1, HealthPerLevel = 5, BaseHealth = 30, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("1bbb40d9-258e-44b6-a9d2-caf941961a15"), Name = "Зомби", RankLevel = 7, HealthPerLevel = 10, BaseHealth = 80, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("ab96b719-c7fc-420e-bfd8-beb35cf533b6"), Name = "Призрак", RankLevel = 6, HealthPerLevel = 10, BaseHealth = 70, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("4e3fb9c4-a451-4cb8-acb1-2030901d8adc"), Name = "Дракон хуеглот", RankLevel = 20, HealthPerLevel = 15, BaseHealth = 200, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("9191d55d-2bfc-4aa8-b064-3a909d666ddc"), Name = "Трогладит", RankLevel = 10, HealthPerLevel = 10, BaseHealth = 100, Sex = MonsterSex.MALE },
            new() { Id = Guid.Parse("1e309681-9a46-4fc6-822f-94f2b8b23a26"), Name = "Ёборотень", RankLevel = 12, HealthPerLevel = 10, BaseHealth = 120, Sex = MonsterSex.MALE },
        };

        var existingIds = (await _context.MonsterMolds
                .Select(m => m.Id)
                .ToListAsync())
            .ToHashSet();

        var existingNames = (await _context.MonsterMolds
                .Select(m => m.Name)
                .ToListAsync())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var monstersToAdd = monsters
            .Where(m => !existingIds.Contains(m.Id) && !existingNames.Contains(m.Name))
            .ToList();

        if (monstersToAdd.Count == 0)
            return;

        _context.MonsterMolds.AddRange(monstersToAdd);
        await _context.SaveChangesAsync();
    }
}
