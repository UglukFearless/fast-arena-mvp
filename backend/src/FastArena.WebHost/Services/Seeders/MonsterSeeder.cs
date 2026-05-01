using FastArena.Dal;
using Microsoft.EntityFrameworkCore;
using FastArena.WebHost.Services.Seeders.Data;

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
        var existingIds = (await _context.MonsterMolds
                .Select(m => m.Id)
                .ToListAsync())
            .ToHashSet();

        var existingNames = (await _context.MonsterMolds
                .Select(m => m.Name)
                .ToListAsync())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var monstersToAdd = MonsterSeedData.All
            .Where(m => !existingIds.Contains(m.Id) && !existingNames.Contains(m.Name))
            .ToList();

        if (monstersToAdd.Count == 0)
            return;

        _context.MonsterMolds.AddRange(monstersToAdd);
        await _context.SaveChangesAsync();
    }
}
