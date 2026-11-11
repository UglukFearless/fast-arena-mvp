using FastArena.Dal;
using FastArena.WebHost.Services.Seeders.Data;
using Microsoft.EntityFrameworkCore;

namespace FastArena.WebHost.Services.Seeders;

public class SlotSeeder
{
    private readonly ApplicationContext _context;

    public SlotSeeder(ApplicationContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!await _context.BodyMaps.AnyAsync())
        {
            var bodyMap = SlotSeedData.HumanoidBodyMap;

            _context.BodyMaps.Add(bodyMap);
            _context.BodyZones.AddRange(bodyMap.Zones!);
        }

        if (!await _context.SlotLayouts.AnyAsync())
        {
            var layout = SlotSeedData.HumanoidSlotLayout;

            _context.SlotLayouts.Add(layout);
            _context.SlotLayoutEntries.AddRange(layout.Entries!);
        }

        await _context.SaveChangesAsync();
    }
}
