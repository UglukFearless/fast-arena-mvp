using FastArena.Dal;
using FastArena.WebHost.Services.Seeders.Data;
using Microsoft.EntityFrameworkCore;

namespace FastArena.WebHost.Services.Seeders;

public class ItemSeeder
{
    private readonly ApplicationContext _context;

    public ItemSeeder(ApplicationContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Items.AnyAsync())
            return;

        _context.Items.AddRange(ItemSeedData.All);
        _context.ItemAllowedSlots.AddRange(ItemSeedData.AllowedSlots);
        await _context.SaveChangesAsync();
    }
}
